using UnityEngine;
using System.Collections.Generic;

namespace RTG
{
    public class CapsuleColliderGizmo3D : GizmoBehaviour
    {
        private class ExtentTick
        {
            public Vector3 Position;
            public Vector3 Normal;
            public GizmoCap2D Tick;
        }

        private CapsuleCollider _targetCollider;
        private bool _scaleFromCenter;
        private bool _isSnapEnabled;

        private float _heightOnDragBegin;

        private GizmoCap2D _rightTick;
        private GizmoCap2D _topTick;
        private GizmoCap2D _backTick;
        private GizmoCap2D _leftTick;
        private GizmoCap2D _bottomTick;
        private GizmoCap2D _frontTick;
        private ExtentTick[] _extentTicks = new ExtentTick[6];

        private List<Vector3> _semiCirclePts = new List<Vector3>();
        private List<Vector3> _circlePts = new List<Vector3>();

        private CapsuleCollider3DSnapshot _preChangeColliderSnapshot = new CapsuleCollider3DSnapshot();
        private CapsuleCollider3DSnapshot _postChangeColliderSnapshot = new CapsuleCollider3DSnapshot();

        private GizmoSglAxisOffsetDrag3D.WorkData _offsetDragWorkData = new GizmoSglAxisOffsetDrag3D.WorkData();
        private GizmoSglAxisOffsetDrag3D _offsetDrag = new GizmoSglAxisOffsetDrag3D();

        private CapsuleColliderGizmo3DLookAndFeel _lookAndFeel = new CapsuleColliderGizmo3DLookAndFeel();
        private CapsuleColliderGizmo3DLookAndFeel _sharedLookAndFeel;

        private CapsuleColliderGizmo3DSettings _settings = new CapsuleColliderGizmo3DSettings();
        private CapsuleColliderGizmo3DSettings _sharedSettings;

        private CapsuleColliderGizmo3DHotkeys _hotkeys = new CapsuleColliderGizmo3DHotkeys();
        private CapsuleColliderGizmo3DHotkeys _sharedHotkeys;

        public CapsuleColliderGizmo3DLookAndFeel LookAndFeel { get { return _sharedLookAndFeel == null ? _lookAndFeel : _sharedLookAndFeel; } }
        public CapsuleColliderGizmo3DLookAndFeel SharedLookAndFeel
        {
            get { return _sharedLookAndFeel; }
            set
            {
                _sharedLookAndFeel = value;
                SetupSharedLookAndFeel();
            }
        }

        public CapsuleColliderGizmo3DSettings Settings { get { return _sharedSettings == null ? _settings : _sharedSettings; } }
        public CapsuleColliderGizmo3DSettings SharedSettings
        {
            get { return _sharedSettings; }
            set { _sharedSettings = value; }
        }

        public CapsuleColliderGizmo3DHotkeys Hotkeys { get { return _sharedHotkeys == null ? _hotkeys : _sharedHotkeys; } }
        public CapsuleColliderGizmo3DHotkeys SharedHotkeys { get { return _sharedHotkeys; } set { _sharedHotkeys = value; } }
        public CapsuleCollider TargetCollider { get { return _targetCollider; } }
        public bool IsSnapEnabled { get { return _isSnapEnabled || Hotkeys.EnableSnapping.IsActive(); } }

        public void SetTargetCollider(CapsuleCollider capsuleCollider)
        {
            _targetCollider = capsuleCollider;
        }

        public override void OnGizmoEnabled()
        {
            OnGizmoUpdateBegin();
        }

        public bool OwnsHandle(int handleId)
        {
            return handleId == _leftTick.HandleId || handleId == _rightTick.HandleId ||
                   handleId == _topTick.HandleId || handleId == _bottomTick.HandleId ||
                   handleId == _frontTick.HandleId || handleId == _backTick.HandleId;
        }

        public void SetSnapEnabled(bool isEnabled)
        {
            _isSnapEnabled = isEnabled;
        }

        public void SetScaleFromCenterEnabled(bool isEnabled)
        {
            _scaleFromCenter = isEnabled;
        }

        public override void OnAttached()
        {
            _leftTick = new GizmoCap2D(Gizmo, GizmoHandleId.NXCap);
            _leftTick.DragSession = _offsetDrag;

            _rightTick = new GizmoCap2D(Gizmo, GizmoHandleId.PXCap);
            _rightTick.DragSession = _offsetDrag;

            _topTick = new GizmoCap2D(Gizmo, GizmoHandleId.PYCap);
            _topTick.DragSession = _offsetDrag;

            _bottomTick = new GizmoCap2D(Gizmo, GizmoHandleId.NYCap);
            _bottomTick.DragSession = _offsetDrag;

            _backTick = new GizmoCap2D(Gizmo, GizmoHandleId.NZCap);
            _backTick.DragSession = _offsetDrag;

            _frontTick = new GizmoCap2D(Gizmo, GizmoHandleId.PZCap);
            _frontTick.DragSession = _offsetDrag;

            _extentTicks[(int)BoxFace.Left] = new ExtentTick();
            _extentTicks[(int)BoxFace.Left].Tick = _leftTick;

            _extentTicks[(int)BoxFace.Right] = new ExtentTick();
            _extentTicks[(int)BoxFace.Right].Tick = _rightTick;

            _extentTicks[(int)BoxFace.Top] = new ExtentTick();
            _extentTicks[(int)BoxFace.Top].Tick = _topTick;

            _extentTicks[(int)BoxFace.Bottom] = new ExtentTick();
            _extentTicks[(int)BoxFace.Bottom].Tick = _bottomTick;

            _extentTicks[(int)BoxFace.Front] = new ExtentTick();
            _extentTicks[(int)BoxFace.Front].Tick = _frontTick;

            _extentTicks[(int)BoxFace.Back] = new ExtentTick();
            _extentTicks[(int)BoxFace.Back].Tick = _backTick;

            _semiCirclePts = PrimitiveFactory.Generate3DArcBorderPoints(Vector3.zero, -Vector3.right, new Plane(Vector3.forward, 0.0f), -180.0f, false, 100);
            _circlePts = PrimitiveFactory.Generate3DCircleBorderPoints(Vector3.zero, 1.0f, Vector3.right, Vector3.up, 100);

            SetupSharedLookAndFeel();
        }

        public override void OnGizmoUpdateBegin()
        {
            if (!IsTargetReady()) return;

            Gizmo.Transform.Position3D = CalcWorldCenter();

            UpdateHandles();
            UpdateHoverPriorities(Gizmo.GetWorkCamera());
        }

        public override void OnGizmoAttemptHandleDragBegin(int handleId)
        {
            if (!IsTargetReady()) return;

            _preChangeColliderSnapshot.Snapshot(_targetCollider);

            if (OwnsHandle(handleId))
            {
                _heightOnDragBegin = _targetCollider.height;

                _offsetDragWorkData.DragOrigin = CalcWorldCenter();
                _offsetDragWorkData.SnapStep = Settings.RadiusSnapStep;

                Quaternion rotationByDir = CalcRotationByDirection();
                Vector3 right = rotationByDir * _targetCollider.transform.right;
                Vector3 up = rotationByDir * _targetCollider.transform.up;
                Vector3 look = rotationByDir * _targetCollider.transform.forward;

                if (handleId == _leftTick.HandleId)
                {
                    _offsetDragWorkData.Axis = -right;
                }
                else
                if (handleId == _rightTick.HandleId)
                {
                    _offsetDragWorkData.Axis = right;
                }
                else
                if (handleId == _topTick.HandleId)
                {
                    _offsetDragWorkData.Axis = up;
                }
                else
                if (handleId == _bottomTick.HandleId)
                {
                    _offsetDragWorkData.Axis = -up;
                }
                else
                if (handleId == _frontTick.HandleId)
                {
                    _offsetDragWorkData.Axis = -look;
                }
                else
                if (handleId == _backTick.HandleId)
                {
                    _offsetDragWorkData.Axis = look;
                }

                _offsetDrag.SetWorkData(_offsetDragWorkData);
            }
        }

        public override void OnGizmoDragUpdate(int handleId)
        {
            if (!IsTargetReady()) return;

            if (OwnsHandle(handleId))
            {
                _offsetDrag.IsSnapEnabled = IsSnapEnabled;

                if (handleId == _bottomTick.HandleId || handleId == _topTick.HandleId)
                {
                    float newWorldHeight = Mathf.Max(0.0f, CalcWorldHeight() + Gizmo.RelativeDragOffset.magnitude * Mathf.Sign(Vector3.Dot(Gizmo.RelativeDragOffset, _offsetDrag.Axis)));
                    float newHeight = newWorldHeight / CalcHeightScale();

                    Vector3 newWorldCenter = CalcScalePivot(handleId) + _offsetDrag.Axis * newWorldHeight * 0.5f;
                    if (!Hotkeys.ScaleFromCenter.IsActive() && !_scaleFromCenter) _targetCollider.center = _targetCollider.transform.InverseTransformPoint(newWorldCenter);

                    _targetCollider.height = newHeight;
                    if (_targetCollider.height < 2.0f * _targetCollider.radius)
                    {
                        _targetCollider.height = 2.0f * _targetCollider.radius;
                        newHeight = _targetCollider.height;

                        float oldWorldHeight = newWorldHeight;
                        newWorldHeight = newHeight * CalcHeightScale();
                        float heightDelta = newWorldHeight - oldWorldHeight;

                        newWorldCenter += _offsetDrag.Axis * heightDelta * 0.5f;
                        if (!Hotkeys.ScaleFromCenter.IsActive() && !_scaleFromCenter) _targetCollider.center = _targetCollider.transform.InverseTransformPoint(newWorldCenter);
                    }
                }
                else
                {
                    float newWorldRadius = Mathf.Max(0.0f, CalcWorldRadius() + Gizmo.RelativeDragOffset.magnitude * Mathf.Sign(Vector3.Dot(Gizmo.RelativeDragOffset, _offsetDrag.Axis)));
                    Vector3 newWorldCenter = CalcScalePivot(handleId) + _offsetDrag.Axis * newWorldRadius;

                    if (!Hotkeys.ScaleFromCenter.IsActive() && !_scaleFromCenter) _targetCollider.center = _targetCollider.transform.InverseTransformPoint(newWorldCenter);

                    _targetCollider.radius = newWorldRadius / CalcRadiusScale();
                    _targetCollider.height = _heightOnDragBegin;
                    if (_targetCollider.height < 2.0f * _targetCollider.radius) _targetCollider.height = 2.0f * _targetCollider.radius;
                }

                UpdateHandles();
            }
        }

        public override void OnGizmoDragEnd(int handleId)
        {
            if (OwnsHandle(handleId))
            {
                _postChangeColliderSnapshot.Snapshot(_targetCollider);
                var action = new CapsuleCollider3DChangedAction(_preChangeColliderSnapshot, _postChangeColliderSnapshot);
                action.Execute();
            }
        }

        public override void OnGizmoRender(Camera camera)
        {
            if (!IsTargetReady()) return;

            if (RTGizmosEngine.Get.NumRenderCameras > 1)
            {
                UpdateHandles();
            }

            Vector3 worldCenter = CalcWorldCenter();
            float worldRadius = CalcWorldRadius();
            float worldHeight = CalcWorldHeight();

            Quaternion rotationByDir = CalcRotationByDirection();
            Vector3 right = rotationByDir * _targetCollider.transform.right;
            Vector3 up = rotationByDir * _targetCollider.transform.up;
            Vector3 look = rotationByDir * _targetCollider.transform.forward;

            var wireMaterial = GizmoLineMaterial.Get;
            wireMaterial.ResetValuesToSensibleDefaults();
            wireMaterial.SetColor(LookAndFeel.WireColor);
            wireMaterial.SetPass(0);

            float lineHeight = worldHeight - 2.0f * worldRadius;
            if (lineHeight > 0.0f)
            {
                GLRenderer.DrawLine3D(worldCenter - right * worldRadius - up * lineHeight * 0.5f, worldCenter - right * worldRadius + up * lineHeight * 0.5f);
                GLRenderer.DrawLine3D(worldCenter + right * worldRadius - up * lineHeight * 0.5f, worldCenter + right * worldRadius + up * lineHeight * 0.5f);
                GLRenderer.DrawLine3D(worldCenter - look * worldRadius - up * lineHeight * 0.5f, worldCenter - look * worldRadius + up * lineHeight * 0.5f);
                GLRenderer.DrawLine3D(worldCenter + look * worldRadius - up * lineHeight * 0.5f, worldCenter + look * worldRadius + up * lineHeight * 0.5f);
            }

            // Top semi-circles and cap circle
            Vector3 circleScale = new Vector3(worldRadius, worldRadius, 1.0f);
            Vector3 semiCircleCenter = worldCenter + up * (worldHeight * 0.5f - worldRadius);
            GL.PushMatrix();
            GL.MultMatrix(Matrix4x4.TRS(semiCircleCenter, rotationByDir, circleScale));
            GLRenderer.DrawLines3D(_semiCirclePts);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.MultMatrix(Matrix4x4.TRS(semiCircleCenter, rotationByDir * Quaternion.Euler(0.0f, 90.0f, 0.0f), circleScale));
            GLRenderer.DrawLines3D(_semiCirclePts);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.MultMatrix(Matrix4x4.TRS(semiCircleCenter, rotationByDir * Quaternion.Euler(90.0f, 0.0f, 0.0f), new Vector3(worldRadius, worldRadius, worldRadius)));
            GLRenderer.DrawLines3D(_circlePts);
            GL.PopMatrix();

            // Bottom semi-circles and cap circle
            semiCircleCenter = worldCenter - up * (worldHeight * 0.5f - worldRadius);
            GL.PushMatrix();
            GL.MultMatrix(Matrix4x4.TRS(semiCircleCenter, rotationByDir * Quaternion.Euler(0.0f, 0.0f, 180.0f), circleScale));
            GLRenderer.DrawLines3D(_semiCirclePts);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.MultMatrix(Matrix4x4.TRS(semiCircleCenter, rotationByDir * Quaternion.Euler(0.0f, 90.0f, 0.0f) * Quaternion.Euler(0.0f, 0.0f, 180.0f), circleScale));
            GLRenderer.DrawLines3D(_semiCirclePts);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.MultMatrix(Matrix4x4.TRS(semiCircleCenter, rotationByDir * Quaternion.Euler(90.0f, 0.0f, 0.0f), new Vector3(worldRadius, worldRadius, worldRadius)));
            GLRenderer.DrawLines3D(_circlePts);
            GL.PopMatrix();

            UpdateTickColors(camera);

            _leftTick.Render(camera);
            _rightTick.Render(camera);
            _topTick.Render(camera);
            _bottomTick.Render(camera);
            _frontTick.Render(camera);
            _backTick.Render(camera);
        }

        private Vector3 CalcScalePivot(int handleId)
        {
            if (OwnsHandle(handleId))
            {
                Vector3 worldCenter = CalcWorldCenter();
                float worldRadius = CalcWorldRadius();
                float worldHeight = CalcWorldHeight();

                Quaternion rotationByDir = CalcRotationByDirection();
                Vector3 right = rotationByDir * _targetCollider.transform.right;
                Vector3 up = rotationByDir * _targetCollider.transform.up;
                Vector3 look = rotationByDir * _targetCollider.transform.forward;

                if (handleId == _leftTick.HandleId) return worldCenter + right * worldRadius;
                else
                if (handleId == _rightTick.HandleId) return worldCenter - right * worldRadius;
                else
                if (handleId == _topTick.HandleId) return worldCenter - up * worldHeight * 0.5f;
                else
                if (handleId == _bottomTick.HandleId) return worldCenter + up * worldHeight * 0.5f;
                else
                if (handleId == _frontTick.HandleId) return worldCenter + look * worldRadius;
                else
                if (handleId == _backTick.HandleId) return worldCenter - look * worldRadius;
            }

            return Vector3.zero;
        }

        private Vector3 CalcWorldCenter()
        {
            return _targetCollider.transform.TransformPoint(_targetCollider.center);
        }

        private float CalcWorldRadius()
        {
            return _targetCollider.radius * CalcRadiusScale();
        }

        private float CalcWorldHeight()
        {
            return _targetCollider.height * CalcHeightScale();
        }

        private float CalcHeightScale()
        {
            if (_targetCollider.direction == 1) return Mathf.Abs(_targetCollider.transform.lossyScale.y);
            else if (_targetCollider.direction == 0) return Mathf.Abs(_targetCollider.transform.lossyScale.x);

            return Mathf.Abs(_targetCollider.transform.lossyScale.z);
        }

        private float CalcRadiusScale()
        {
            Vector3 scale = _targetCollider.transform.lossyScale.Abs();
            if (_targetCollider.direction == 1) return Mathf.Max(scale.x, scale.z);
            else if (_targetCollider.direction == 0) return Mathf.Max(scale.y, scale.z);

            return Mathf.Max(scale.x, scale.y);
        }

        private Quaternion CalcRotationByDirection()
        {
            if (_targetCollider.direction == 1) return Quaternion.identity;
            else if (_targetCollider.direction == 0) return Quaternion.Euler(0.0f, 0.0f, 90.0f);

            return Quaternion.Euler(90.0f, 0.0f, 0.0f);
        }

        private void UpdateHandles()
        {
            Camera camera = Gizmo.GetWorkCamera();
            Vector3 worldCenter = CalcWorldCenter();
            float worldRadius = CalcWorldRadius();
            float worldHeight = CalcWorldHeight();

            Quaternion rotationByDir = CalcRotationByDirection();
            Vector3 right = rotationByDir * _targetCollider.transform.right;
            Vector3 up = rotationByDir * _targetCollider.transform.up;
            Vector3 look = rotationByDir * _targetCollider.transform.forward;

            Vector3 extentPosition = worldCenter - right * worldRadius;
            _leftTick.Position = camera.WorldToScreenPoint(extentPosition);
            _extentTicks[(int)BoxFace.Left].Position = extentPosition;
            _extentTicks[(int)BoxFace.Left].Normal = -right;

            extentPosition = worldCenter + right * worldRadius;
            _rightTick.Position = camera.WorldToScreenPoint(extentPosition);
            _extentTicks[(int)BoxFace.Right].Position = extentPosition;
            _extentTicks[(int)BoxFace.Right].Normal = right;

            extentPosition = worldCenter + up * worldHeight * 0.5f;
            _topTick.Position = camera.WorldToScreenPoint(extentPosition);
            _extentTicks[(int)BoxFace.Top].Position = extentPosition;
            _extentTicks[(int)BoxFace.Top].Normal = up;

            extentPosition = worldCenter - up * worldHeight * 0.5f;
            _bottomTick.Position = camera.WorldToScreenPoint(extentPosition);
            _extentTicks[(int)BoxFace.Bottom].Position = extentPosition;
            _extentTicks[(int)BoxFace.Bottom].Normal = -up;

            extentPosition = worldCenter - look * worldRadius;
            _frontTick.Position = camera.WorldToScreenPoint(extentPosition);
            _extentTicks[(int)BoxFace.Front].Position = extentPosition;
            _extentTicks[(int)BoxFace.Front].Normal = -look;

            extentPosition = worldCenter + look * worldRadius;
            _backTick.Position = camera.WorldToScreenPoint(extentPosition);
            _extentTicks[(int)BoxFace.Back].Position = extentPosition;
            _extentTicks[(int)BoxFace.Back].Normal = look;
        }

        private void UpdateTickColors(Camera camera)
        {
            Plane cullPlane = new Plane(camera.transform.forward, camera.transform.position);
            foreach (var extentTick in _extentTicks)
            {
                var tick = extentTick.Tick;
                if (Gizmo.HoverHandleId != tick.HandleId && !camera.IsPointFacingCamera(extentTick.Position, extentTick.Normal))
                {
                    tick.OverrideFillColor.IsActive = true;
                    tick.OverrideBorderColor.IsActive = true;

                    Color color = tick.SharedLookAndFeel.Color;
                    tick.OverrideFillColor.Color = color.KeepAllButAlpha(LookAndFeel.TickCullAlphaScale * color.a);

                    color = tick.SharedLookAndFeel.BorderColor;
                    tick.OverrideBorderColor.Color = color.KeepAllButAlpha(LookAndFeel.TickCullAlphaScale * color.a);
                }
                else
                {
                    tick.OverrideFillColor.IsActive = false;
                    tick.OverrideBorderColor.IsActive = false;
                }

                if (cullPlane.GetDistanceToPoint(extentTick.Position) > 0.0f) tick.SetVisible(true);
                else tick.SetVisible(false);
            }
        }

        private void UpdateHoverPriorities(Camera camera)
        {
            int basePriority = 0;

            var faceTick0 = _extentTicks[(int)BoxFace.Left];
            var faceTick1 = _extentTicks[(int)BoxFace.Right];
            faceTick0.Tick.HoverPriority2D.Value = basePriority;
            faceTick1.Tick.HoverPriority2D.Value = basePriority;
            if (camera.IsPointFacingCamera(faceTick0.Position, faceTick0.Normal))
                faceTick0.Tick.HoverPriority2D.MakeHigherThan(faceTick1.Tick.HoverPriority2D);
            else
                faceTick1.Tick.HoverPriority2D.MakeHigherThan(faceTick0.Tick.HoverPriority2D);

            basePriority += 2;
            faceTick0 = _extentTicks[(int)BoxFace.Top];
            faceTick1 = _extentTicks[(int)BoxFace.Bottom];
            faceTick0.Tick.HoverPriority2D.Value = basePriority;
            faceTick1.Tick.HoverPriority2D.Value = basePriority;
            if (camera.IsPointFacingCamera(faceTick0.Position, faceTick0.Normal))
                faceTick0.Tick.HoverPriority2D.MakeHigherThan(faceTick1.Tick.HoverPriority2D);
            else
                faceTick1.Tick.HoverPriority2D.MakeHigherThan(faceTick0.Tick.HoverPriority2D);

            basePriority += 2;
            faceTick0 = _extentTicks[(int)BoxFace.Front];
            faceTick1 = _extentTicks[(int)BoxFace.Back];
            faceTick0.Tick.HoverPriority2D.Value = basePriority;
            faceTick1.Tick.HoverPriority2D.Value = basePriority;
            if (camera.IsPointFacingCamera(faceTick0.Position, faceTick0.Normal))
                faceTick0.Tick.HoverPriority2D.MakeHigherThan(faceTick1.Tick.HoverPriority2D);
            else
                faceTick1.Tick.HoverPriority2D.MakeHigherThan(faceTick0.Tick.HoverPriority2D);
        }

        private void SetupSharedLookAndFeel()
        {
            LookAndFeel.ConnectTickLookAndFeel(_rightTick, 0, AxisSign.Positive);
            LookAndFeel.ConnectTickLookAndFeel(_topTick, 1, AxisSign.Positive);
            LookAndFeel.ConnectTickLookAndFeel(_backTick, 2, AxisSign.Positive);
            LookAndFeel.ConnectTickLookAndFeel(_leftTick, 0, AxisSign.Negative);
            LookAndFeel.ConnectTickLookAndFeel(_bottomTick, 1, AxisSign.Negative);
            LookAndFeel.ConnectTickLookAndFeel(_frontTick, 2, AxisSign.Negative);
        }

        private bool IsTargetReady()
        {
            return _targetCollider != null && _targetCollider.enabled && _targetCollider.gameObject.activeSelf;
        }
    }
}
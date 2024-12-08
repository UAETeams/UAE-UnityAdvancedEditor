using UnityEngine;

namespace RTG
{
    public class SphereColliderGizmo : GizmoBehaviour
    {
        private class ExtentTick
        {
            public Vector3 Position;
            public Vector3 Normal;
            public GizmoCap2D Tick;
        }

        private SphereCollider _targetCollider;
        private bool _scaleFromCenter;
        private bool _isSnapEnabled;

        private GizmoCap2D _rightTick;
        private GizmoCap2D _topTick;
        private GizmoCap2D _backTick;
        private GizmoCap2D _leftTick;
        private GizmoCap2D _bottomTick;
        private GizmoCap2D _frontTick;
        private ExtentTick[] _extentTicks = new ExtentTick[6];

        private GizmoPlaneSlider3D _axialCircleXY;
        private GizmoPlaneSlider3D _axialCircleYZ;
        private GizmoPlaneSlider3D _axialCircleZX;

        private PolygonShape2D _sphereBorderPoly = new PolygonShape2D();

        private SphereColliderSnapshot _preChangeColliderSnapshot = new SphereColliderSnapshot();
        private SphereColliderSnapshot _postChangeColliderSnapshot = new SphereColliderSnapshot();

        private GizmoSglAxisOffsetDrag3D.WorkData _offsetDragWorkData = new GizmoSglAxisOffsetDrag3D.WorkData();
        private GizmoSglAxisOffsetDrag3D _offsetDrag = new GizmoSglAxisOffsetDrag3D();

        private SphereColliderGizmoLookAndFeel _lookAndFeel = new SphereColliderGizmoLookAndFeel();
        private SphereColliderGizmoLookAndFeel _sharedLookAndFeel;

        private SphereColliderGizmoSettings _settings = new SphereColliderGizmoSettings();
        private SphereColliderGizmoSettings _sharedSettings;

        private SphereColliderGizmoHotkeys _hotkeys = new SphereColliderGizmoHotkeys();
        private SphereColliderGizmoHotkeys _sharedHotkeys;

        public SphereColliderGizmoLookAndFeel LookAndFeel { get { return _sharedLookAndFeel == null ? _lookAndFeel : _sharedLookAndFeel; } }
        public SphereColliderGizmoLookAndFeel SharedLookAndFeel
        {
            get { return _sharedLookAndFeel; }
            set
            {
                _sharedLookAndFeel = value;
                SetupSharedLookAndFeel();
            }
        }

        public SphereColliderGizmoSettings Settings { get { return _sharedSettings == null ? _settings : _sharedSettings; } }
        public SphereColliderGizmoSettings SharedSettings
        {
            get { return _sharedSettings; }
            set { _sharedSettings = value; }
        }

        public SphereColliderGizmoHotkeys Hotkeys { get { return _sharedHotkeys == null ? _hotkeys : _sharedHotkeys; } }
        public SphereColliderGizmoHotkeys SharedHotkeys { get { return _sharedHotkeys; } set { _sharedHotkeys = value; } }
        public SphereCollider TargetCollider { get { return _targetCollider; } }
        public bool IsSnapEnabled { get { return _isSnapEnabled || Hotkeys.EnableSnapping.IsActive(); } }

        public void SetTargetCollider(SphereCollider sphereCollider)
        {
            _targetCollider = sphereCollider;
        }

        public override void OnGizmoEnabled()
        {
            OnGizmoUpdateBegin();
        }

        public bool OwnsHandle(int handleId)
        {
            return handleId == _leftTick.HandleId || handleId == _rightTick.HandleId ||
                   handleId == _topTick.HandleId || handleId == _bottomTick.HandleId ||
                   handleId == _frontTick.HandleId || handleId == _backTick.HandleId ||
                   handleId == _axialCircleXY.HandleId || handleId == _axialCircleYZ.HandleId ||
                   handleId == _axialCircleZX.HandleId;
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

            _axialCircleXY = new GizmoPlaneSlider3D(Gizmo, GizmoHandleId.XYDblSlider);
            _axialCircleXY.SetVisible(false);
            _axialCircleXY.SetBorderHoverable(false);
            _axialCircleXY.LookAndFeel.UseZoomFactor = false;
            _axialCircleXY.LookAndFeel.PlaneType = GizmoPlane3DType.Circle;

            _axialCircleYZ = new GizmoPlaneSlider3D(Gizmo, GizmoHandleId.YZDblSlider);
            _axialCircleYZ.SetVisible(false);
            _axialCircleYZ.SetBorderHoverable(false);
            _axialCircleYZ.LookAndFeel.UseZoomFactor = false;
            _axialCircleYZ.LookAndFeel.PlaneType = GizmoPlane3DType.Circle;

            _axialCircleZX = new GizmoPlaneSlider3D(Gizmo, GizmoHandleId.ZXDblSlider);
            _axialCircleZX.SetVisible(false);
            _axialCircleZX.SetBorderHoverable(false);
            _axialCircleZX.LookAndFeel.UseZoomFactor = false;
            _axialCircleZX.LookAndFeel.PlaneType = GizmoPlane3DType.Circle;

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
                _offsetDragWorkData.DragOrigin = CalcWorldCenter();
                _offsetDragWorkData.SnapStep = Settings.RadiusSnapStep;

                if (handleId == _leftTick.HandleId)
                {
                    _offsetDragWorkData.Axis = -_targetCollider.transform.right;
                }
                else
                if (handleId == _rightTick.HandleId)
                {
                    _offsetDragWorkData.Axis = _targetCollider.transform.right;
                }
                else
                if (handleId == _topTick.HandleId)
                {
                    _offsetDragWorkData.Axis = _targetCollider.transform.up;
                }
                else
                if (handleId == _bottomTick.HandleId)
                {
                    _offsetDragWorkData.Axis = -_targetCollider.transform.up;
                }
                else
                if (handleId == _frontTick.HandleId)
                {
                    _offsetDragWorkData.Axis = -_targetCollider.transform.forward;
                }
                else
                if (handleId == _backTick.HandleId)
                {
                    _offsetDragWorkData.Axis = _targetCollider.transform.forward;
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

                float newWorldRadius = Mathf.Max(0.0f, CalcWorldRadius() + Gizmo.RelativeDragOffset.magnitude * Mathf.Sign(Vector3.Dot(Gizmo.RelativeDragOffset, _offsetDrag.Axis)));
                Vector3 newWorldCenter = CalcScalePivot(handleId) + _offsetDrag.Axis * newWorldRadius;

                if (!Hotkeys.ScaleFromCenter.IsActive() && !_scaleFromCenter) _targetCollider.center = _targetCollider.transform.InverseTransformPoint(newWorldCenter);
                _targetCollider.radius = newWorldRadius / CalcMaxTransformAbsScale();

                UpdateHandles();
            }
        }

        public override void OnGizmoDragEnd(int handleId)
        {
            if (OwnsHandle(handleId))
            {
                _postChangeColliderSnapshot.Snapshot(_targetCollider);
                var action = new SphereColliderChangedAction(_preChangeColliderSnapshot, _postChangeColliderSnapshot);
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

            UpdateTickColors(camera);

            Vector3 worldCenter = CalcWorldCenter();
            var boxWireMaterial = GizmoLineMaterial.Get;
            boxWireMaterial.ResetValuesToSensibleDefaults();
            boxWireMaterial.SetColor(LookAndFeel.SphereBorderColor);
            boxWireMaterial.SetPass(0);
            _sphereBorderPoly.MakeSphereBorder(worldCenter, CalcWorldRadius(), 100, camera);
            _sphereBorderPoly.RenderBorder(camera);

            _axialCircleXY.Render(camera);
            _axialCircleYZ.Render(camera);
            _axialCircleZX.Render(camera);

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

                if (handleId == _leftTick.HandleId) return worldCenter + _targetCollider.transform.right * worldRadius;
                else
                if (handleId == _rightTick.HandleId) return worldCenter - _targetCollider.transform.right * worldRadius;
                else
                if (handleId == _topTick.HandleId) return worldCenter - _targetCollider.transform.up * worldRadius;
                else
                if (handleId == _bottomTick.HandleId) return worldCenter + _targetCollider.transform.up * worldRadius;
                else
                if (handleId == _frontTick.HandleId) return worldCenter + _targetCollider.transform.forward * worldRadius;
                else
                if (handleId == _backTick.HandleId) return worldCenter - _targetCollider.transform.forward * worldRadius;
            }

            return Vector3.zero;
        }

        private Vector3 CalcWorldCenter()
        {
            return _targetCollider.transform.TransformPoint(_targetCollider.center);
        }

        private float CalcWorldRadius()
        {
            return _targetCollider.radius * CalcMaxTransformAbsScale();
        }

        private float CalcMaxTransformAbsScale()
        {
            Vector3 scale = _targetCollider.transform.lossyScale.Abs();
            float largestAbsScale = scale.x;
            if (largestAbsScale < scale.y) largestAbsScale = scale.y;
            if (largestAbsScale < scale.z) largestAbsScale = scale.z;

            return largestAbsScale;
        }

        private void UpdateHandles()
        {
            Camera camera = Gizmo.GetWorkCamera();
            Vector3 worldCenter = CalcWorldCenter();
            float worldRadius = CalcWorldRadius();

            Vector3 extentPosition = worldCenter - _targetCollider.transform.right * worldRadius;
            _leftTick.Position = camera.WorldToScreenPoint(extentPosition);
            _extentTicks[(int)BoxFace.Left].Position = extentPosition;
            _extentTicks[(int)BoxFace.Left].Normal = -_targetCollider.transform.right;

            extentPosition = worldCenter + _targetCollider.transform.right * worldRadius;
            _rightTick.Position = camera.WorldToScreenPoint(extentPosition);
            _extentTicks[(int)BoxFace.Right].Position = extentPosition;
            _extentTicks[(int)BoxFace.Right].Normal = _targetCollider.transform.right;

            extentPosition = worldCenter + _targetCollider.transform.up * worldRadius;
            _topTick.Position = camera.WorldToScreenPoint(extentPosition);
            _extentTicks[(int)BoxFace.Top].Position = extentPosition;
            _extentTicks[(int)BoxFace.Top].Normal = _targetCollider.transform.up;

            extentPosition = worldCenter - _targetCollider.transform.up * worldRadius;
            _bottomTick.Position = camera.WorldToScreenPoint(extentPosition);
            _extentTicks[(int)BoxFace.Bottom].Position = extentPosition;
            _extentTicks[(int)BoxFace.Bottom].Normal = -_targetCollider.transform.up;

            extentPosition = worldCenter - _targetCollider.transform.forward * worldRadius;
            _frontTick.Position = camera.WorldToScreenPoint(extentPosition);
            _extentTicks[(int)BoxFace.Front].Position = extentPosition;
            _extentTicks[(int)BoxFace.Front].Normal = -_targetCollider.transform.forward;

            extentPosition = worldCenter + _targetCollider.transform.forward * worldRadius;
            _backTick.Position = camera.WorldToScreenPoint(extentPosition);
            _extentTicks[(int)BoxFace.Back].Position = extentPosition;
            _extentTicks[(int)BoxFace.Back].Normal = _targetCollider.transform.forward;

            _axialCircleXY.Position = worldCenter;
            _axialCircleYZ.Position = worldCenter;
            _axialCircleZX.Position = worldCenter;

            _axialCircleXY.Rotation = _targetCollider.transform.rotation;
            _axialCircleYZ.Rotation = _targetCollider.transform.rotation * Quaternion.Euler(0.0f, 90.0f, 0.0f);
            _axialCircleZX.Rotation = _targetCollider.transform.rotation * Quaternion.Euler(90.0f, 0.0f, 0.0f);

            _axialCircleXY.LookAndFeel.BorderColor = LookAndFeel.WireColor;
            _axialCircleYZ.LookAndFeel.BorderColor = LookAndFeel.WireColor;
            _axialCircleZX.LookAndFeel.BorderColor = LookAndFeel.WireColor;

            _axialCircleXY.LookAndFeel.CircleRadius = worldRadius;
            _axialCircleYZ.LookAndFeel.CircleRadius = worldRadius;
            _axialCircleZX.LookAndFeel.CircleRadius = worldRadius;

            _axialCircleXY.LookAndFeel.BorderCircleCullAlphaScale = LookAndFeel.AxialCircleCullAlphaScale;
            _axialCircleYZ.LookAndFeel.BorderCircleCullAlphaScale = LookAndFeel.AxialCircleCullAlphaScale;
            _axialCircleZX.LookAndFeel.BorderCircleCullAlphaScale = LookAndFeel.AxialCircleCullAlphaScale;

            _axialCircleXY.Refresh();
            _axialCircleYZ.Refresh();
            _axialCircleZX.Refresh();
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

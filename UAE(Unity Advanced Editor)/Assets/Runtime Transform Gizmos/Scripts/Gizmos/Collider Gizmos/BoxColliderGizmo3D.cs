using System;
using UnityEngine;

namespace RTG
{
    public class BoxColliderGizmo3D : GizmoBehaviour
    {
        private class FaceTick
        {
            public Vector3 FaceCenter;
            public Vector3 FaceNormal;
            public GizmoCap2D Tick;
        }

        private BoxCollider _targetCollider;
        private bool _scaleFromCenter;
        private bool _isMidCapVisible = true;
        private bool _isSnapEnabled;

        private GizmoCap2D _rightTick;
        private GizmoCap2D _topTick;
        private GizmoCap2D _backTick;
        private GizmoCap2D _leftTick;
        private GizmoCap2D _bottomTick;
        private GizmoCap2D _frontTick;
        private FaceTick[] _faceTicks = new FaceTick[6];
        private GizmoCap3D _midCap;

        private BoxCollider3DSnapshot _preChangeColliderSnapshot = new BoxCollider3DSnapshot();
        private BoxCollider3DSnapshot _postChangeColliderSnapshot = new BoxCollider3DSnapshot();

        private GizmoSglAxisOffsetDrag3D.WorkData _offsetDragWorkData = new GizmoSglAxisOffsetDrag3D.WorkData();
        private GizmoSglAxisOffsetDrag3D _offsetDrag = new GizmoSglAxisOffsetDrag3D();
        private Vector3 _scalePivot;
        private int _dragAxisIndex = -1;

        private GizmoUniformScaleDrag3D.WorkData _uniScaleDragWorkData = new GizmoUniformScaleDrag3D.WorkData();
        private GizmoUniformScaleDrag3D _uniScaleDrag = new GizmoUniformScaleDrag3D();

        private BoxColliderGizmo3DLookAndFeel _lookAndFeel = new BoxColliderGizmo3DLookAndFeel();
        private BoxColliderGizmo3DLookAndFeel _sharedLookAndFeel;

        private BoxColliderGizmo3DSettings _settings = new BoxColliderGizmo3DSettings();
        private BoxColliderGizmo3DSettings _sharedSettings;

        private BoxColliderGizmo3DHotkeys _hotkeys = new BoxColliderGizmo3DHotkeys();
        private BoxColliderGizmo3DHotkeys _sharedHotkeys;

        public BoxColliderGizmo3DLookAndFeel LookAndFeel { get { return _sharedLookAndFeel == null ? _lookAndFeel : _sharedLookAndFeel; } }
        public BoxColliderGizmo3DLookAndFeel SharedLookAndFeel
        {
            get { return _sharedLookAndFeel; }
            set
            {
                _sharedLookAndFeel = value;
                SetupSharedLookAndFeel();
            }
        }

        public BoxColliderGizmo3DSettings Settings { get { return _sharedSettings == null ? _settings : _sharedSettings; } }
        public BoxColliderGizmo3DSettings SharedSettings
        {
            get { return _sharedSettings; }
            set { _sharedSettings = value; }
        }

        public BoxColliderGizmo3DHotkeys Hotkeys { get { return _sharedHotkeys == null ? _hotkeys : _sharedHotkeys; } }
        public BoxColliderGizmo3DHotkeys SharedHotkeys { get { return _sharedHotkeys; } set { _sharedHotkeys = value; } }
        public BoxCollider TargetCollider { get { return _targetCollider; } }
        public bool IsSnapEnabled { get { return _isSnapEnabled || Hotkeys.EnableSnapping.IsActive(); } }

        public void SetTargetCollider(BoxCollider boxCollider)
        {
            _targetCollider = boxCollider;
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
                   handleId == _midCap.HandleId;
        }

        public void SetSnapEnabled(bool isEnabled)
        {
            _isSnapEnabled = isEnabled;
        }

        public void SetScaleFromCenterEnabled(bool isEnabled)
        {
            _scaleFromCenter = isEnabled;
        }

        public void SetMidCapVisible(bool visible)
        {
            _isMidCapVisible = visible;
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

            _faceTicks[(int)BoxFace.Left] = new FaceTick();
            _faceTicks[(int)BoxFace.Left].Tick = _leftTick;

            _faceTicks[(int)BoxFace.Right] = new FaceTick();
            _faceTicks[(int)BoxFace.Right].Tick = _rightTick;

            _faceTicks[(int)BoxFace.Top] = new FaceTick();
            _faceTicks[(int)BoxFace.Top].Tick = _topTick;

            _faceTicks[(int)BoxFace.Bottom] = new FaceTick();
            _faceTicks[(int)BoxFace.Bottom].Tick = _bottomTick;

            _faceTicks[(int)BoxFace.Front] = new FaceTick();
            _faceTicks[(int)BoxFace.Front].Tick = _frontTick;

            _faceTicks[(int)BoxFace.Back] = new FaceTick();
            _faceTicks[(int)BoxFace.Back].Tick = _backTick;

            _midCap = new GizmoCap3D(Gizmo, GizmoHandleId.MidScaleCap);
            _midCap.DragSession = _uniScaleDrag;

            SetupSharedLookAndFeel();
        }

        public override void OnGizmoUpdateBegin()
        {
            if (!IsTargetReady()) return;

            Gizmo.Transform.Position3D = CalcWorldCenter();

            UpdateTicks();
            UpdateHoverPriorities(Gizmo.GetWorkCamera());

            _midCap.Position = Gizmo.Transform.Position3D;
            _midCap.SetVisible(LookAndFeel.IsMidCapVisible && _isMidCapVisible);
        }

        public override void OnGizmoAttemptHandleDragBegin(int handleId)
        {
            if (!IsTargetReady()) return;

            _preChangeColliderSnapshot.Snapshot(_targetCollider);

            if (OwnsHandle(handleId))
            {
                if (handleId == _midCap.HandleId)
                {
                    _uniScaleDragWorkData.CameraRight = Gizmo.GetWorkCamera().transform.right;
                    _uniScaleDragWorkData.CameraUp = Gizmo.GetWorkCamera().transform.up;
                    _uniScaleDragWorkData.DragOrigin = CalcWorldCenter();
                    _uniScaleDragWorkData.SnapStep = Settings.UniformSizeSnapStep;
                    _uniScaleDrag.SetWorkData(_uniScaleDragWorkData);
                }
                else
                {
                    _offsetDragWorkData.DragOrigin = CalcWorldCenter();
                    _scalePivot = CalcScalePivot(handleId);

                    if (handleId == _leftTick.HandleId)
                    {
                        _offsetDragWorkData.Axis = -_targetCollider.transform.right;
                        _offsetDragWorkData.SnapStep = Settings.XSizeSnapStep;
                        _dragAxisIndex = 0;
                    }
                    else
                    if (handleId == _rightTick.HandleId)
                    {
                        _offsetDragWorkData.Axis = _targetCollider.transform.right;
                        _offsetDragWorkData.SnapStep = Settings.XSizeSnapStep;
                        _dragAxisIndex = 0;
                    }
                    else
                    if (handleId == _topTick.HandleId)
                    {
                        _offsetDragWorkData.Axis = _targetCollider.transform.up;
                        _offsetDragWorkData.SnapStep = Settings.YSizeSnapStep;
                        _dragAxisIndex = 1;
                    }
                    else
                    if (handleId == _bottomTick.HandleId)
                    {
                        _offsetDragWorkData.Axis = -_targetCollider.transform.up;
                        _offsetDragWorkData.SnapStep = Settings.YSizeSnapStep;
                        _dragAxisIndex = 1;
                    }
                    else
                    if (handleId == _frontTick.HandleId)
                    {
                        _offsetDragWorkData.Axis = -_targetCollider.transform.forward;
                        _offsetDragWorkData.SnapStep = Settings.ZSizeSnapStep;
                        _dragAxisIndex = 2;
                    }
                    else
                    if (handleId == _backTick.HandleId)
                    {
                        _offsetDragWorkData.Axis = _targetCollider.transform.forward;
                        _offsetDragWorkData.SnapStep = Settings.ZSizeSnapStep;
                        _dragAxisIndex = 2;
                    }

                    _offsetDrag.SetWorkData(_offsetDragWorkData);
                }
            }
        }

        public override void OnGizmoDragUpdate(int handleId)
        {
            if (!IsTargetReady()) return;

            if (OwnsHandle(handleId))
            {
                _uniScaleDrag.IsSnapEnabled = IsSnapEnabled;
                _offsetDrag.IsSnapEnabled = IsSnapEnabled;

                if (handleId == _midCap.HandleId)
                {
                    Vector3 newBoxWorldSize = Vector3.Scale(Gizmo.RelativeDragScale, CalcWorldSize()).Abs();
                    if (newBoxWorldSize == Vector3.zero) newBoxWorldSize = Vector3Ex.FromValue(1e-8f);
                    _targetCollider.size = Vector3.Scale(newBoxWorldSize, _targetCollider.transform.lossyScale.GetInverse());
                }
                else
                {
                    Vector3 newBoxWorldSize = CalcWorldSize();
                    float dragOffset = Vector3.Dot(Gizmo.RelativeDragOffset, _offsetDrag.Axis); 
                    newBoxWorldSize[_dragAxisIndex] = Math.Max(0.0f, newBoxWorldSize[_dragAxisIndex] + Mathf.Abs(dragOffset) * Mathf.Sign(dragOffset));
                    Vector3 newBoxWorldCenter = _scalePivot + _offsetDrag.Axis * (newBoxWorldSize[_dragAxisIndex] * 0.5f);

                    if (!Hotkeys.ScaleFromCenter.IsActive() && !_scaleFromCenter) _targetCollider.center = _targetCollider.transform.InverseTransformPoint(newBoxWorldCenter);
                    _targetCollider.size = Vector3.Scale(newBoxWorldSize, _targetCollider.transform.lossyScale.GetInverse());
                }

                UpdateTicks();
            }
        }

        public override void OnGizmoDragEnd(int handleId)
        {
            if (OwnsHandle(handleId))
            {
                _postChangeColliderSnapshot.Snapshot(_targetCollider);
                var action = new BoxCollider3DChangedAction(_preChangeColliderSnapshot, _postChangeColliderSnapshot);
                action.Execute();
            }
        }

        public override void OnGizmoRender(Camera camera)
        {
            if (!IsTargetReady()) return;

            var boxWireMaterial = GizmoLineMaterial.Get;
            boxWireMaterial.ResetValuesToSensibleDefaults();
            boxWireMaterial.SetColor(LookAndFeel.WireColor);
            boxWireMaterial.SetPass(0);
            GraphicsEx.DrawWireBox(new OBB(CalcWorldCenter(), CalcWorldSize(), _targetCollider.transform.rotation));

            if (RTGizmosEngine.Get.NumRenderCameras > 1)
            {
                UpdateTicks();
            }

            UpdateTickColors(camera);

            _leftTick.Render(camera);
            _rightTick.Render(camera);
            _topTick.Render(camera);
            _bottomTick.Render(camera);
            _frontTick.Render(camera);
            _backTick.Render(camera);

            _midCap.Render(camera);
        }

        private Vector3 CalcScalePivot(int handleId)
        {
            if (OwnsHandle(handleId))
            {
                Vector3 boxWorldCenter = CalcWorldCenter();
                Vector3 boxWorldSize = CalcWorldSize();
                Quaternion boxWorldRotation = _targetCollider.transform.rotation;

                if (handleId == _leftTick.HandleId) return BoxMath.CalcBoxFaceCenter(boxWorldCenter, boxWorldSize, boxWorldRotation, BoxFace.Right);
                else
                if (handleId == _rightTick.HandleId) return BoxMath.CalcBoxFaceCenter(boxWorldCenter, boxWorldSize, boxWorldRotation, BoxFace.Left);
                else
                if (handleId == _topTick.HandleId) return BoxMath.CalcBoxFaceCenter(boxWorldCenter, boxWorldSize, boxWorldRotation, BoxFace.Bottom);
                else
                if (handleId == _bottomTick.HandleId) return BoxMath.CalcBoxFaceCenter(boxWorldCenter, boxWorldSize, boxWorldRotation, BoxFace.Top);
                else
                if (handleId == _frontTick.HandleId) return BoxMath.CalcBoxFaceCenter(boxWorldCenter, boxWorldSize, boxWorldRotation, BoxFace.Back);
                else
                if (handleId == _backTick.HandleId) return BoxMath.CalcBoxFaceCenter(boxWorldCenter, boxWorldSize, boxWorldRotation, BoxFace.Front);
            }

            return Vector3.zero;
        }

        private Vector3 CalcWorldCenter()
        {
            return _targetCollider.transform.TransformPoint(_targetCollider.center);
        }

        private Vector3 CalcWorldSize()
        {
            return Vector3.Scale(_targetCollider.size, _targetCollider.transform.lossyScale);
        }

        private void UpdateTicks()
        {
            Camera camera = Gizmo.GetWorkCamera();
            Vector3 boxCenter = CalcWorldCenter();
            Vector3 boxSize = CalcWorldSize();
            Quaternion rotation = _targetCollider.transform.rotation;

            Vector3 faceCenter = BoxMath.CalcBoxFaceCenter(boxCenter, boxSize, rotation, BoxFace.Left);
            _leftTick.Position = camera.WorldToScreenPoint(faceCenter);
            _faceTicks[(int)BoxFace.Left].FaceCenter = faceCenter;
            _faceTicks[(int)BoxFace.Left].FaceNormal = -_targetCollider.transform.right;

            faceCenter = BoxMath.CalcBoxFaceCenter(boxCenter, boxSize, rotation, BoxFace.Right);
            _rightTick.Position = camera.WorldToScreenPoint(faceCenter);
            _faceTicks[(int)BoxFace.Right].FaceCenter = faceCenter;
            _faceTicks[(int)BoxFace.Right].FaceNormal = _targetCollider.transform.right;

            faceCenter = BoxMath.CalcBoxFaceCenter(boxCenter, boxSize, rotation, BoxFace.Top);
            _topTick.Position = camera.WorldToScreenPoint(faceCenter);
            _faceTicks[(int)BoxFace.Top].FaceCenter = faceCenter;
            _faceTicks[(int)BoxFace.Top].FaceNormal = _targetCollider.transform.up;

            faceCenter = BoxMath.CalcBoxFaceCenter(boxCenter, boxSize, rotation, BoxFace.Bottom);
            _bottomTick.Position = camera.WorldToScreenPoint(faceCenter);
            _faceTicks[(int)BoxFace.Bottom].FaceCenter = faceCenter;
            _faceTicks[(int)BoxFace.Bottom].FaceNormal = -_targetCollider.transform.up;

            faceCenter = BoxMath.CalcBoxFaceCenter(boxCenter, boxSize, rotation, BoxFace.Front);
            _frontTick.Position = camera.WorldToScreenPoint(faceCenter);
            _faceTicks[(int)BoxFace.Front].FaceCenter = faceCenter;
            _faceTicks[(int)BoxFace.Front].FaceNormal = -_targetCollider.transform.forward;

            faceCenter = BoxMath.CalcBoxFaceCenter(boxCenter, boxSize, rotation, BoxFace.Back);
            _backTick.Position = camera.WorldToScreenPoint(faceCenter);
            _faceTicks[(int)BoxFace.Back].FaceCenter = faceCenter;
            _faceTicks[(int)BoxFace.Back].FaceNormal = _targetCollider.transform.forward;
        }

        private void UpdateTickColors(Camera camera)
        {
            Plane cullPlane = new Plane(camera.transform.forward, camera.transform.position);
            foreach (var faceTick in _faceTicks)
            {
                var tick = faceTick.Tick;
                if (Gizmo.HoverHandleId != tick.HandleId && !camera.IsPointFacingCamera(faceTick.FaceCenter, faceTick.FaceNormal))
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

                if (cullPlane.GetDistanceToPoint(faceTick.FaceCenter) > 0.0f) tick.SetVisible(true);
                else tick.SetVisible(false);
            }
        }

        private void UpdateHoverPriorities(Camera camera)
        {
            int basePriority = 0;

            var faceTick0 = _faceTicks[(int)BoxFace.Left];
            var faceTick1 = _faceTicks[(int)BoxFace.Right];
            faceTick0.Tick.HoverPriority2D.Value = basePriority;
            faceTick1.Tick.HoverPriority2D.Value = basePriority;
            if (camera.IsPointFacingCamera(faceTick0.FaceCenter, faceTick0.FaceNormal))
                faceTick0.Tick.HoverPriority2D.MakeHigherThan(faceTick1.Tick.HoverPriority2D);
            else
                faceTick1.Tick.HoverPriority2D.MakeHigherThan(faceTick0.Tick.HoverPriority2D);

            _midCap.GenericHoverPriority.MakeHigherThan(faceTick0.Tick.GenericHoverPriority);
            _midCap.GenericHoverPriority.MakeHigherThan(faceTick1.Tick.GenericHoverPriority);

            basePriority += 2;
            faceTick0 = _faceTicks[(int)BoxFace.Top];
            faceTick1 = _faceTicks[(int)BoxFace.Bottom];
            faceTick0.Tick.HoverPriority2D.Value = basePriority;
            faceTick1.Tick.HoverPriority2D.Value = basePriority;
            if (camera.IsPointFacingCamera(faceTick0.FaceCenter, faceTick0.FaceNormal))
                faceTick0.Tick.HoverPriority2D.MakeHigherThan(faceTick1.Tick.HoverPriority2D);
            else
                faceTick1.Tick.HoverPriority2D.MakeHigherThan(faceTick0.Tick.HoverPriority2D);

            _midCap.GenericHoverPriority.MakeHigherThan(faceTick0.Tick.GenericHoverPriority);
            _midCap.GenericHoverPriority.MakeHigherThan(faceTick1.Tick.GenericHoverPriority);

            basePriority += 2;
            faceTick0 = _faceTicks[(int)BoxFace.Front];
            faceTick1 = _faceTicks[(int)BoxFace.Back];
            faceTick0.Tick.HoverPriority2D.Value = basePriority;
            faceTick1.Tick.HoverPriority2D.Value = basePriority;
            if (camera.IsPointFacingCamera(faceTick0.FaceCenter, faceTick0.FaceNormal))
                faceTick0.Tick.HoverPriority2D.MakeHigherThan(faceTick1.Tick.HoverPriority2D);
            else
                faceTick1.Tick.HoverPriority2D.MakeHigherThan(faceTick0.Tick.HoverPriority2D);

            _midCap.GenericHoverPriority.MakeHigherThan(faceTick0.Tick.GenericHoverPriority);
            _midCap.GenericHoverPriority.MakeHigherThan(faceTick1.Tick.GenericHoverPriority);
        }

        private void SetupSharedLookAndFeel()
        {
            LookAndFeel.ConnectTickLookAndFeel(_rightTick, 0, AxisSign.Positive);
            LookAndFeel.ConnectTickLookAndFeel(_topTick, 1, AxisSign.Positive);
            LookAndFeel.ConnectTickLookAndFeel(_backTick, 2, AxisSign.Positive);
            LookAndFeel.ConnectTickLookAndFeel(_leftTick, 0, AxisSign.Negative);
            LookAndFeel.ConnectTickLookAndFeel(_bottomTick, 1, AxisSign.Negative);
            LookAndFeel.ConnectTickLookAndFeel(_frontTick, 2, AxisSign.Negative);
            LookAndFeel.ConnectMidCapLookAndFeel(_midCap);
        }

        private bool IsTargetReady()
        {
            return _targetCollider != null && _targetCollider.enabled && _targetCollider.gameObject.activeSelf;
        }
    }
}

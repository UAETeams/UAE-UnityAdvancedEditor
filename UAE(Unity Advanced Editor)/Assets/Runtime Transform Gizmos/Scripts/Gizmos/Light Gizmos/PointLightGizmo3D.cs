using UnityEngine;

namespace RTG
{
    public class PointLightGizmo3D : GizmoBehaviour
    {
        private class ExtentTick
        {
            public Vector3 Position;
            public Vector3 Normal;
            public GizmoCap2D Tick;
        }

        private Light _targetLight;
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

        private Light3DSnapshot _preChangeSnapshot = new Light3DSnapshot();
        private Light3DSnapshot _postChangeSnapshot = new Light3DSnapshot();

        private GizmoSglAxisOffsetDrag3D.WorkData _offsetDragWorkData = new GizmoSglAxisOffsetDrag3D.WorkData();
        private GizmoSglAxisOffsetDrag3D _offsetDrag = new GizmoSglAxisOffsetDrag3D();

        private PointLightGizmo3DLookAndFeel _lookAndFeel = new PointLightGizmo3DLookAndFeel();
        private PointLightGizmo3DLookAndFeel _sharedLookAndFeel;

        private PointLightGizmo3DSettings _settings = new PointLightGizmo3DSettings();
        private PointLightGizmo3DSettings _sharedSettings;

        private PointLightGizmo3DHotkeys _hotkeys = new PointLightGizmo3DHotkeys();
        private PointLightGizmo3DHotkeys _sharedHotkeys;

        public PointLightGizmo3DLookAndFeel LookAndFeel { get { return _sharedLookAndFeel == null ? _lookAndFeel : _sharedLookAndFeel; } }
        public PointLightGizmo3DLookAndFeel SharedLookAndFeel
        {
            get { return _sharedLookAndFeel; }
            set
            {
                _sharedLookAndFeel = value;
                SetupSharedLookAndFeel();
            }
        }

        public PointLightGizmo3DSettings Settings { get { return _sharedSettings == null ? _settings : _sharedSettings; } }
        public PointLightGizmo3DSettings SharedSettings
        {
            get { return _sharedSettings; }
            set { _sharedSettings = value; }
        }

        public PointLightGizmo3DHotkeys Hotkeys { get { return _sharedHotkeys == null ? _hotkeys : _sharedHotkeys; } }
        public PointLightGizmo3DHotkeys SharedHotkeys { get { return _sharedHotkeys; } set { _sharedHotkeys = value; } }
        public Light TargetLight { get { return _targetLight; } }
        public bool IsSnapEnabled { get { return _isSnapEnabled || Hotkeys.EnableSnapping.IsActive(); } }

        public void SetTargetLight(Light targetLight)
        {
            _targetLight = targetLight;
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

            Gizmo.Transform.Position3D = _targetLight.transform.position;

            UpdateHandles();
            UpdateHoverPriorities(Gizmo.GetWorkCamera());
        }

        public override void OnGizmoAttemptHandleDragBegin(int handleId)
        {
            if (!IsTargetReady()) return;

            _preChangeSnapshot.Snapshot(_targetLight);

            if (OwnsHandle(handleId))
            {
                _offsetDragWorkData.DragOrigin = _targetLight.transform.position;
                _offsetDragWorkData.SnapStep = Settings.RadiusSnapStep;

                if (handleId == _leftTick.HandleId)
                {
                    _offsetDragWorkData.Axis = -Vector3.right;
                }
                else
                if (handleId == _rightTick.HandleId)
                {
                    _offsetDragWorkData.Axis = Vector3.right;
                }
                else
                if (handleId == _topTick.HandleId)
                {
                    _offsetDragWorkData.Axis = Vector3.up;
                }
                else
                if (handleId == _bottomTick.HandleId)
                {
                    _offsetDragWorkData.Axis = -Vector3.up;
                }
                else
                if (handleId == _frontTick.HandleId)
                {
                    _offsetDragWorkData.Axis = -Vector3.forward;
                }
                else
                if (handleId == _backTick.HandleId)
                {
                    _offsetDragWorkData.Axis = Vector3.forward;
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

                _targetLight.range = Mathf.Max(0.0f, _targetLight.range + Gizmo.RelativeDragOffset.magnitude * Mathf.Sign(Vector3.Dot(Gizmo.RelativeDragOffset, _offsetDrag.Axis)));
                UpdateHandles();
            }
        }

        public override void OnGizmoDragEnd(int handleId)
        {
            if (OwnsHandle(handleId))
            {
                _postChangeSnapshot.Snapshot(_targetLight);
                var action = new Light3DChangedAction(_preChangeSnapshot, _postChangeSnapshot);
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

            Plane nearPlane = new Plane(camera.transform.forward, camera.transform.position + camera.transform.forward * camera.nearClipPlane);
            Sphere sphere = new Sphere(_targetLight.transform.position, _targetLight.range);
            if (!sphere.ContainsPoint(camera.transform.position) && nearPlane.GetDistanceToPoint(_targetLight.transform.position) > 0.0f)
            {
                var boxWireMaterial = GizmoLineMaterial.Get;
                boxWireMaterial.ResetValuesToSensibleDefaults();
                boxWireMaterial.SetColor(LookAndFeel.SphereBorderColor);
                boxWireMaterial.SetPass(0);
                _sphereBorderPoly.MakeSphereBorder(_targetLight.transform.position, _targetLight.range, 100, camera);
                _sphereBorderPoly.RenderBorder(camera);
            }

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
                Vector3 worldCenter = _targetLight.transform.position;
                float worldRadius = _targetLight.range;

                if (handleId == _leftTick.HandleId) return worldCenter + Vector3.right * worldRadius;
                else
                if (handleId == _rightTick.HandleId) return worldCenter - Vector3.right * worldRadius;
                else
                if (handleId == _topTick.HandleId) return worldCenter - Vector3.up * worldRadius;
                else
                if (handleId == _bottomTick.HandleId) return worldCenter + Vector3.up * worldRadius;
                else
                if (handleId == _frontTick.HandleId) return worldCenter + Vector3.forward * worldRadius;
                else
                if (handleId == _backTick.HandleId) return worldCenter - Vector3.forward * worldRadius;
            }

            return Vector3.zero;
        }

        private void UpdateHandles()
        {
            Camera camera = Gizmo.GetWorkCamera();
            Vector3 worldCenter = _targetLight.transform.position;
            float range = _targetLight.range;

            Vector3 extentPosition = worldCenter - Vector3.right * range;
            _leftTick.Position = camera.WorldToScreenPoint(extentPosition);
            _extentTicks[(int)BoxFace.Left].Position = extentPosition;
            _extentTicks[(int)BoxFace.Left].Normal = -Vector3.right;

            extentPosition = worldCenter + Vector3.right * range;
            _rightTick.Position = camera.WorldToScreenPoint(extentPosition);
            _extentTicks[(int)BoxFace.Right].Position = extentPosition;
            _extentTicks[(int)BoxFace.Right].Normal = Vector3.right;

            extentPosition = worldCenter + Vector3.up * range;
            _topTick.Position = camera.WorldToScreenPoint(extentPosition);
            _extentTicks[(int)BoxFace.Top].Position = extentPosition;
            _extentTicks[(int)BoxFace.Top].Normal = Vector3.up;

            extentPosition = worldCenter - Vector3.up * range;
            _bottomTick.Position = camera.WorldToScreenPoint(extentPosition);
            _extentTicks[(int)BoxFace.Bottom].Position = extentPosition;
            _extentTicks[(int)BoxFace.Bottom].Normal = -Vector3.up;

            extentPosition = worldCenter - Vector3.forward * range;
            _frontTick.Position = camera.WorldToScreenPoint(extentPosition);
            _extentTicks[(int)BoxFace.Front].Position = extentPosition;
            _extentTicks[(int)BoxFace.Front].Normal = -Vector3.forward;

            extentPosition = worldCenter + Vector3.forward * range;
            _backTick.Position = camera.WorldToScreenPoint(extentPosition);
            _extentTicks[(int)BoxFace.Back].Position = extentPosition;
            _extentTicks[(int)BoxFace.Back].Normal = Vector3.forward;

            _axialCircleXY.Position = worldCenter;
            _axialCircleYZ.Position = worldCenter;
            _axialCircleZX.Position = worldCenter;

            _axialCircleXY.Rotation = Quaternion.identity;
            _axialCircleYZ.Rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            _axialCircleZX.Rotation = Quaternion.Euler(90.0f, 0.0f, 0.0f);

            _axialCircleXY.LookAndFeel.BorderColor = LookAndFeel.WireColor;
            _axialCircleYZ.LookAndFeel.BorderColor = LookAndFeel.WireColor;
            _axialCircleZX.LookAndFeel.BorderColor = LookAndFeel.WireColor;

            _axialCircleXY.LookAndFeel.CircleRadius = range;
            _axialCircleYZ.LookAndFeel.CircleRadius = range;
            _axialCircleZX.LookAndFeel.CircleRadius = range;

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
            return _targetLight != null && _targetLight.enabled && _targetLight.gameObject.activeSelf && _targetLight.type == LightType.Point;
        }
    }
}
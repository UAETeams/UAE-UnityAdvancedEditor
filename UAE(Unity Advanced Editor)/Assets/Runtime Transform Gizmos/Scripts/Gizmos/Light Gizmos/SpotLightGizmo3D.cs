using UnityEngine;
using System.Collections.Generic;

namespace RTG
{
    public class SpotLightGizmo3D : GizmoBehaviour
    {
        private enum AngleTickId
        {
            Top = 0,
            Right,
            Bottom,
            Left
        }

        private class AngleTick
        {
            public Vector3 Position;
            public Vector3 LightAxis;
            public GizmoCap2D Tick;
        }

        private Light _targetLight;
        private Vector3 _pickedWorldSnapPoint;
        private bool _isSnapEnabled;

        private List<Vector3> _coneCirclePoints = new List<Vector3>();

        private GizmoCap2D _dirSnapTick;
        private SceneRaycastFilter _raycastFilter = new SceneRaycastFilter();

        private AngleTick[] _angleTicks = new AngleTick[4];

        private GizmoCap2D _rangeTick;

        private GizmoSglAxisOffsetDrag3D _dummyDragSession = new GizmoSglAxisOffsetDrag3D();
        private GizmoSglAxisOffsetDrag3D.WorkData _dummySessionWorkData = new GizmoSglAxisOffsetDrag3D.WorkData();

        private GizmoSglAxisOffsetDrag3D _sglAxisDrag = new GizmoSglAxisOffsetDrag3D();
        private GizmoSglAxisOffsetDrag3D.WorkData _sglAxisDragWorkData = new GizmoSglAxisOffsetDrag3D.WorkData();

        private Light3DSnapshot _preChangeSnapshot = new Light3DSnapshot();
        private Light3DSnapshot _postChangeSnapshot = new Light3DSnapshot();

        private SpotLightGizmo3DLookAndFeel _lookAndFeel = new SpotLightGizmo3DLookAndFeel();
        private SpotLightGizmo3DLookAndFeel _sharedLookAndFeel;

        private SpotLightGizmo3DSettings _settings = new SpotLightGizmo3DSettings();
        private SpotLightGizmo3DSettings _sharedSettings;

        private SpotLightGizmo3DHotkeys _hotkeys = new SpotLightGizmo3DHotkeys();
        private SpotLightGizmo3DHotkeys _sharedHotkeys;

        public SpotLightGizmo3DLookAndFeel LookAndFeel { get { return _sharedLookAndFeel == null ? _lookAndFeel : _sharedLookAndFeel; } }
        public SpotLightGizmo3DLookAndFeel SharedLookAndFeel
        {
            get { return _sharedLookAndFeel; }
            set
            {
                _sharedLookAndFeel = value;
                SetupSharedLookAndFeel();
            }
        }
        public SpotLightGizmo3DSettings Settings { get { return _sharedSettings == null ? _settings : _sharedSettings; } }
        public SpotLightGizmo3DSettings SharedSettings
        {
            get { return _sharedSettings; }
            set
            {
                _sharedSettings = value;
            }
        }
        public SpotLightGizmo3DHotkeys Hotkeys { get { return _sharedHotkeys == null ? _hotkeys : _sharedHotkeys; } }
        public SpotLightGizmo3DHotkeys SharedHotkeys
        {
            get { return _sharedHotkeys; }
            set
            {
                _sharedHotkeys = value;
            }
        }
        public Light TargetLight { get { return _targetLight; } }
        public bool IsSnapEnabled { get { return _isSnapEnabled || Hotkeys.EnableSnapping.IsActive(); } }

        public void SetTargetLight(Light targetLight)
        {
            _targetLight = targetLight;
        }

        public void SetSnapEnabled(bool isEnabled)
        {
            _isSnapEnabled = isEnabled;
        }

        public bool OwnsHandle(int handleId)
        {
            foreach(var angleTick in _angleTicks)
            {
                if (angleTick.Tick.HandleId == handleId) return true;
            }

            return handleId == _dirSnapTick.HandleId || handleId == _rangeTick.HandleId;
        }

        public override void OnAttached()
        {
            _dirSnapTick = new GizmoCap2D(Gizmo, GizmoHandleId.DirectionSnapCap);
            _dirSnapTick.DragSession = _dummyDragSession;

            int tickIndex = (int)AngleTickId.Bottom;
            _angleTicks[tickIndex] = new AngleTick();
            _angleTicks[tickIndex].Tick = new GizmoCap2D(Gizmo, GizmoHandleId.SpotAngleCapBottom);
            _angleTicks[tickIndex].Tick.DragSession = _sglAxisDrag;

            tickIndex = (int)AngleTickId.Top;
            _angleTicks[tickIndex] = new AngleTick();
            _angleTicks[tickIndex].Tick = new GizmoCap2D(Gizmo, GizmoHandleId.SpotAngleCapTop);
            _angleTicks[tickIndex].Tick.DragSession = _sglAxisDrag;

            tickIndex = (int)AngleTickId.Left;
            _angleTicks[tickIndex] = new AngleTick();
            _angleTicks[tickIndex].Tick = new GizmoCap2D(Gizmo, GizmoHandleId.SpotAngleCapLeft);
            _angleTicks[tickIndex].Tick.DragSession = _sglAxisDrag;

            tickIndex = (int)AngleTickId.Right;
            _angleTicks[tickIndex] = new AngleTick();
            _angleTicks[tickIndex].Tick = new GizmoCap2D(Gizmo, GizmoHandleId.SpotAngleCapRight);
            _angleTicks[tickIndex].Tick.DragSession = _sglAxisDrag;

            _rangeTick = new GizmoCap2D(Gizmo, GizmoHandleId.RangeTick);
            _rangeTick.DragSession = _sglAxisDrag;

            SetupSharedLookAndFeel();

            _coneCirclePoints = PrimitiveFactory.Generate3DCircleBorderPoints(Vector3.zero, 1.0f, Vector3.right, Vector3.up, 100);

            _raycastFilter.AllowedObjectTypes.Add(GameObjectType.Mesh);
            _raycastFilter.AllowedObjectTypes.Add(GameObjectType.Terrain);
        }

        public override void OnGizmoUpdateBegin()
        {
            if (!IsTargetReady()) return;

            Gizmo.Transform.Position3D = _targetLight.transform.position;

            UpdateTicks();
        }

        public override void OnGizmoAttemptHandleDragBegin(int handleId)
        {
            if (!OwnsHandle(handleId)) return;
            if (!IsTargetReady()) return;

            if (handleId == _dirSnapTick.HandleId)
            {
                _dummySessionWorkData.Axis = Vector3.one;
                _dummyDragSession.SetWorkData(_dummySessionWorkData);
            }
            else
            {
                if (handleId == _angleTicks[(int)AngleTickId.Left].Tick.HandleId)
                {
                    _sglAxisDragWorkData.Axis = _angleTicks[(int)AngleTickId.Left].LightAxis;
                    _sglAxisDragWorkData.DragOrigin = CalcConeBase();
                    _sglAxisDragWorkData.SnapStep = Settings.RadiusSnapStep;
                }
                else
                if (handleId == _angleTicks[(int)AngleTickId.Right].Tick.HandleId)
                {
                    _sglAxisDragWorkData.Axis = _angleTicks[(int)AngleTickId.Right].LightAxis;
                    _sglAxisDragWorkData.DragOrigin = CalcConeBase();
                    _sglAxisDragWorkData.SnapStep = Settings.RadiusSnapStep;
                }
                else
                if (handleId == _angleTicks[(int)AngleTickId.Top].Tick.HandleId)
                {
                    _sglAxisDragWorkData.Axis = _angleTicks[(int)AngleTickId.Top].LightAxis;
                    _sglAxisDragWorkData.DragOrigin = CalcConeBase();
                    _sglAxisDragWorkData.SnapStep = Settings.RadiusSnapStep;
                }
                else
                if (handleId == _angleTicks[(int)AngleTickId.Bottom].Tick.HandleId)
                {
                    _sglAxisDragWorkData.Axis = _angleTicks[(int)AngleTickId.Bottom].LightAxis;
                    _sglAxisDragWorkData.DragOrigin = CalcConeBase();
                    _sglAxisDragWorkData.SnapStep = Settings.RadiusSnapStep;
                }
                else 
                if (handleId == _rangeTick.HandleId)
                {
                    _sglAxisDragWorkData.Axis = _targetLight.transform.forward;
                    _sglAxisDragWorkData.DragOrigin = CalcConeBase();
                    _sglAxisDragWorkData.SnapStep = Settings.RangeSnapStep;
                }

                _sglAxisDrag.SetWorkData(_sglAxisDragWorkData);
            }

            _preChangeSnapshot.Snapshot(_targetLight);
        }

        public override void OnGizmoDragUpdate(int handleId)
        {
            if (!OwnsHandle(handleId)) return;
            if (!IsTargetReady()) return;

            _sglAxisDrag.IsSnapEnabled = IsSnapEnabled;

            if (handleId == _dirSnapTick.HandleId) SnapDirection();
            else
            {
                if (handleId >= GizmoHandleId.SpotAngleCapTop && handleId <= GizmoHandleId.SpotAngleCapRight)
                {
                    _sglAxisDrag.IsSnapEnabled = Hotkeys.EnableSnapping.IsActive();
                    float coneRadius = CalcConeRadius() + Gizmo.RelativeDragOffset.magnitude * Mathf.Sign(Vector3.Dot(Gizmo.RelativeDragOffset, _sglAxisDrag.Axis));

                    _targetLight.spotAngle = CalcSpotAngleDegrees(coneRadius) * 2.0f;
                }
                else
                if (handleId == _rangeTick.HandleId)
                {
                    _sglAxisDrag.IsSnapEnabled = Hotkeys.EnableSnapping.IsActive();
                    _targetLight.range += Gizmo.RelativeDragOffset.magnitude * Mathf.Sign(Vector3.Dot(Gizmo.RelativeDragOffset, _sglAxisDrag.Axis));
                }
            }

            UpdateTicks();
        }

        public override void OnGizmoDragEnd(int handleId)
        {
            _postChangeSnapshot.Snapshot(_targetLight);
            var action = new Light3DChangedAction(_preChangeSnapshot, _postChangeSnapshot);
            action.Execute();
        }

        public override void OnGizmoRender(Camera camera)
        {
            if (!IsTargetReady()) return;

            Vector3 lightPos = _targetLight.transform.position;
            Vector3 coneBase = CalcConeBase();
            float coneRadius = CalcConeRadius();

            if (RTGizmosEngine.Get.NumRenderCameras > 1)
            {
                UpdateTicks();
            }

            var wireMaterial = GizmoLineMaterial.Get;
            wireMaterial.ResetValuesToSensibleDefaults();
            wireMaterial.SetColor(LookAndFeel.WireColor);
            wireMaterial.SetPass(0);

            GL.PushMatrix();
            GL.MultMatrix(Matrix4x4.TRS(coneBase, _targetLight.transform.rotation, new Vector3(coneRadius, coneRadius, 1.0f)));
            GLRenderer.DrawLines3D(_coneCirclePoints);
            GL.PopMatrix();

            GLRenderer.DrawLine3D(lightPos, coneBase + _targetLight.transform.up * coneRadius);
            GLRenderer.DrawLine3D(lightPos, coneBase + _targetLight.transform.right * coneRadius);
            GLRenderer.DrawLine3D(lightPos, coneBase - _targetLight.transform.up * coneRadius);
            GLRenderer.DrawLine3D(lightPos, coneBase - _targetLight.transform.right * coneRadius);

            if (Gizmo.DragHandleId == _dirSnapTick.HandleId)
            {
                wireMaterial.SetColor(LookAndFeel.DirSnapSegmentColor);
                wireMaterial.SetPass(0);

                GLRenderer.DrawLine3D(lightPos, _pickedWorldSnapPoint);
            }

            foreach(var angleTick in _angleTicks)
            {
                angleTick.Tick.Render(camera);
            }

            _rangeTick.Render(camera);
            _dirSnapTick.Render(camera);
        }

        private float CalcConeRadius()
        {
            return Mathf.Tan(Mathf.Deg2Rad * _targetLight.spotAngle * 0.5f) * _targetLight.range;
        }

        private Vector3 CalcConeBase()
        {
            return _targetLight.transform.position + _targetLight.transform.forward * _targetLight.range;
        }

        private float CalcSpotAngleDegrees(float radius)
        {
            return Mathf.Atan2(radius, _targetLight.range) * Mathf.Rad2Deg;
        }

        private void UpdateTicks()
        {
            Camera camera = Gizmo.GetWorkCamera();
            Plane cullPlane = new Plane(camera.transform.forward, camera.transform.position);

            if (Gizmo.DragHandleId == _dirSnapTick.HandleId)
            {
                if (cullPlane.GetDistanceToPoint(_pickedWorldSnapPoint) > 0.0f) _dirSnapTick.SetVisible(true);
                else _dirSnapTick.SetVisible(false);
                _dirSnapTick.Position = Gizmo.GetWorkCamera().WorldToScreenPoint(_pickedWorldSnapPoint);
            }
            else
            {
                Vector3 lightPos = _targetLight.transform.position;
                _dirSnapTick.Position = Gizmo.GetWorkCamera().WorldToScreenPoint(lightPos);
                if (cullPlane.GetDistanceToPoint(lightPos) > 0.0f) _dirSnapTick.SetVisible(true);
                else _dirSnapTick.SetVisible(false);
            }

            Vector3 coneBase = CalcConeBase();
            float coneRadius = CalcConeRadius();
            Vector3 right = _targetLight.transform.right;
            Vector3 up = _targetLight.transform.up;

            _rangeTick.Position = camera.WorldToScreenPoint(coneBase);
            if (cullPlane.GetDistanceToPoint(coneBase) > 0.0f) _rangeTick.SetVisible(true);
            else _rangeTick.SetVisible(false);

            int tickIndex = (int)AngleTickId.Bottom;
            _angleTicks[tickIndex].Position = coneBase - up * coneRadius;
            _angleTicks[tickIndex].LightAxis = -up;
            _angleTicks[tickIndex].Tick.Position = camera.WorldToScreenPoint(_angleTicks[tickIndex].Position);
            if (cullPlane.GetDistanceToPoint(_angleTicks[tickIndex].Position) > 0.0f) _angleTicks[tickIndex].Tick.SetVisible(true);
            else _angleTicks[tickIndex].Tick.SetVisible(false);

            tickIndex = (int)AngleTickId.Top;
            _angleTicks[tickIndex].Position = coneBase + up * coneRadius;
            _angleTicks[tickIndex].LightAxis = up;
            _angleTicks[tickIndex].Tick.Position = camera.WorldToScreenPoint(_angleTicks[tickIndex].Position);
            if (cullPlane.GetDistanceToPoint(_angleTicks[tickIndex].Position) > 0.0f) _angleTicks[tickIndex].Tick.SetVisible(true);
            else _angleTicks[tickIndex].Tick.SetVisible(false);

            tickIndex = (int)AngleTickId.Left;
            _angleTicks[tickIndex].Position = coneBase - right * coneRadius;
            _angleTicks[tickIndex].LightAxis = -right;
            _angleTicks[tickIndex].Tick.Position = camera.WorldToScreenPoint(_angleTicks[tickIndex].Position);
            if (cullPlane.GetDistanceToPoint(_angleTicks[tickIndex].Position) > 0.0f) _angleTicks[tickIndex].Tick.SetVisible(true);
            else _angleTicks[tickIndex].Tick.SetVisible(false);

            tickIndex = (int)AngleTickId.Right;
            _angleTicks[tickIndex].Position = coneBase + right * coneRadius;
            _angleTicks[tickIndex].LightAxis = right;
            _angleTicks[tickIndex].Tick.Position = camera.WorldToScreenPoint(_angleTicks[tickIndex].Position);
            if (cullPlane.GetDistanceToPoint(_angleTicks[tickIndex].Position) > 0.0f) _angleTicks[tickIndex].Tick.SetVisible(true);
            else _angleTicks[tickIndex].Tick.SetVisible(false);
        }

        private void SetupSharedLookAndFeel()
        {
            LookAndFeel.ConnectDirSnapTickLookAndFeel(_dirSnapTick);
            LookAndFeel.ConnectTickLookAndFeel(_rangeTick);

            foreach (var angleTick in _angleTicks)
                LookAndFeel.ConnectTickLookAndFeel(angleTick.Tick);
        }

        private void SnapDirection()
        {
            SceneRaycastHit rayHit = RTScene.Get.Raycast(RTInputDevice.Get.Device.GetRay(Gizmo.GetWorkCamera()), SceneRaycastPrecision.BestFit, _raycastFilter);
            if (rayHit.WasAnObjectHit) _pickedWorldSnapPoint = rayHit.ObjectHit.HitPoint;
            else if (rayHit.WasGridHit) _pickedWorldSnapPoint = rayHit.GridHit.HitPoint;
            else return;

            Vector3 targetDir = _pickedWorldSnapPoint - _targetLight.transform.position;
            if (targetDir.magnitude > 1e-4f) _targetLight.transform.Align(targetDir.normalized, TransformAxis.PositiveZ);
        }

        private bool IsTargetReady()
        {
            return _targetLight != null && _targetLight.enabled && _targetLight.gameObject.activeSelf && _targetLight.type == LightType.Spot;
        }
    }
}
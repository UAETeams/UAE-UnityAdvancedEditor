using UnityEngine;
using System.Collections.Generic;

namespace RTG
{
    public class DirectionalLightGizmo3D : GizmoBehaviour
    {
        private Light _targetLight;
        private Vector3 _pickedWorldSnapPoint;

        private GizmoCap2D _dirSnapTick;
        private List<Vector3> _sourceCirclePoints;
        private List<Vector3> _lightRayEmissionPoints = new List<Vector3>();

        private SceneRaycastFilter _raycastFilter = new SceneRaycastFilter();

        private GizmoSglAxisOffsetDrag3D _dummyDragSession = new GizmoSglAxisOffsetDrag3D();
        private GizmoSglAxisOffsetDrag3D.WorkData _dummySessionWorkData = new GizmoSglAxisOffsetDrag3D.WorkData();

        private Light3DSnapshot _preChangeSnapshot = new Light3DSnapshot();
        private Light3DSnapshot _postChangeSnapshot = new Light3DSnapshot();

        private DirectionalLightGizmo3DLookAndFeel _lookAndFeel = new DirectionalLightGizmo3DLookAndFeel();
        private DirectionalLightGizmo3DLookAndFeel _sharedLookAndFeel;

        public DirectionalLightGizmo3DLookAndFeel LookAndFeel { get { return _sharedLookAndFeel == null ? _lookAndFeel : _sharedLookAndFeel; } }
        public DirectionalLightGizmo3DLookAndFeel SharedLookAndFeel
        {
            get { return _sharedLookAndFeel; }
            set
            {
                _sharedLookAndFeel = value;
                SetupSharedLookAndFeel();
            }
        }
        public Light TargetLight { get { return _targetLight; } }

        public void SetTargetLight(Light targetLight)
        {
            _targetLight = targetLight;
        }

        public bool OwnsHandle(int handleId)
        {
            return handleId == _dirSnapTick.HandleId;
        }

        public override void OnAttached()
        {
            _dirSnapTick = new GizmoCap2D(Gizmo, GizmoHandleId.DirectionSnapCap);
            _dirSnapTick.DragSession = _dummyDragSession;

            SetupSharedLookAndFeel();

            _sourceCirclePoints = PrimitiveFactory.Generate3DCircleBorderPoints(Vector3.zero, 1.0f, Vector3.right, Vector3.up, 100);

            _raycastFilter.AllowedObjectTypes.Add(GameObjectType.Mesh);
            _raycastFilter.AllowedObjectTypes.Add(GameObjectType.Terrain);
        }

        public override void OnGizmoUpdateBegin()
        {
            if (!IsTargetReady()) return;

            Gizmo.Transform.Position3D = _targetLight.transform.position;

            UpdateTicks(Gizmo.GetWorkCamera());
        }

        public override void OnGizmoAttemptHandleDragBegin(int handleId)
        {
            if (!OwnsHandle(handleId)) return;
            if (!IsTargetReady()) return;

            _dummySessionWorkData.Axis = Vector3.one;
            _dummyDragSession.SetWorkData(_dummySessionWorkData);

            _preChangeSnapshot.Snapshot(_targetLight);
        }

        public override void OnGizmoDragUpdate(int handleId)
        {
            if (!IsTargetReady()) return;

            if (handleId == _dirSnapTick.HandleId) SnapDirection();
            UpdateTicks(Gizmo.GetWorkCamera());
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
            float zoomFactor = camera.EstimateZoomFactor(lightPos);
            float circleRadius = zoomFactor * LookAndFeel.SourceCircleRadius;

            if (RTGizmosEngine.Get.NumRenderCameras > 1)
            {
                UpdateTicks(camera);
            }

            var wireMaterial = GizmoLineMaterial.Get;
            wireMaterial.ResetValuesToSensibleDefaults();
            wireMaterial.SetColor(LookAndFeel.SourceCircleBorderColor);
            wireMaterial.SetPass(0);

            GL.PushMatrix();
            GL.MultMatrix(Matrix4x4.TRS(lightPos, _targetLight.transform.rotation, new Vector3(circleRadius, circleRadius, 1.0f)));
            GLRenderer.DrawLines3D(_sourceCirclePoints);
            GL.PopMatrix();

            if (Gizmo.DragHandleId == _dirSnapTick.HandleId)
            {
                wireMaterial.SetColor(LookAndFeel.DirSnapSegmentColor);
                wireMaterial.SetPass(0);

                GLRenderer.DrawLine3D(_targetLight.transform.position, _pickedWorldSnapPoint);
            }

            wireMaterial.SetColor(LookAndFeel.LightRaysColor);
            wireMaterial.SetPass(0);

            Vector3 look = _targetLight.transform.forward;
            GenerateLightRayEmissionPoints(camera);
            float rayLength = zoomFactor * LookAndFeel.LightRayLength;
            foreach(var pt in _lightRayEmissionPoints)
                GLRenderer.DrawLine3D(pt, pt + look * rayLength);

            _dirSnapTick.Render(camera);
        }

        private void UpdateTicks(Camera camera)
        {
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
                if (cullPlane.GetDistanceToPoint(lightPos) > 0.0f) _dirSnapTick.SetVisible(true);
                else _dirSnapTick.SetVisible(false);
                _dirSnapTick.Position = Gizmo.GetWorkCamera().WorldToScreenPoint(lightPos);
            }
        }

        private void SetupSharedLookAndFeel()
        {
            LookAndFeel.ConnectDirSnapTickLookAndFeel(_dirSnapTick);
        }

        private void GenerateLightRayEmissionPoints(Camera camera)
        {
            Vector3 circleCenter = _targetLight.transform.position;
            float zoomFactor = camera.EstimateZoomFactor(circleCenter);
            float circleRadius = zoomFactor * LookAndFeel.SourceCircleRadius;
            Vector3 circleRight = _targetLight.transform.right;
            Vector3 circleUp = _targetLight.transform.up;

            _lightRayEmissionPoints.Clear();

            float angleStep = 360.0f / (LookAndFeel.NumLightRays - 1);
            for (int ptIndex = 0; ptIndex < LookAndFeel.NumLightRays; ++ptIndex)
            {
                float angle = angleStep * ptIndex * Mathf.Deg2Rad;
                _lightRayEmissionPoints.Add(circleCenter + circleRight * Mathf.Sin(angle) * circleRadius + circleUp * Mathf.Cos(angle) * circleRadius);
            }
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
            return _targetLight != null && _targetLight.enabled && _targetLight.gameObject.activeSelf && _targetLight.type == LightType.Directional;
        }
    }
}
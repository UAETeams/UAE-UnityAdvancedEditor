using UnityEngine;
using System;
using System.Collections.Generic;

namespace RTG
{
    public class TerrainGizmo : GizmoBehaviour
    {
        [Flags]
        public enum TargetTypeFlags
        {
            Terrain = 1,
            ObjectsInRadius = 2,
            All = Terrain | ObjectsInRadius
        }

        private class RadiusTick
        {
            public GizmoCap2D Tick;
            public Vector3 DragAxis;
            public Vector3 WorldPosition;
        }

        private class ObjectRotationData
        {
            public bool RotatingObjects;
            public List<GameObject> GameObjects = new List<GameObject>();
            public List<LocalTransformSnapshot> PreSnapshots;
        }

        private struct Patch
        {
            public int MinCol, MaxCol;
            public int MinDepth, MaxDepth;

            public void Clamp(int heightmapRes)
            {
                MinCol = Mathf.Clamp(MinCol, 0, heightmapRes - 1);
                MaxCol = Mathf.Clamp(MaxCol, 0, heightmapRes - 1);
                MinDepth = Mathf.Clamp(MinDepth, 0, heightmapRes - 1);
                MaxDepth = Mathf.Clamp(MaxDepth, 0, heightmapRes - 1);
            }
        }

        private TargetTypeFlags _targetTypeFlags = TargetTypeFlags.All;
        private bool _isSnapEnabled;
        private bool _isVisible = false;
        private Terrain _targetTerrain;
        private TerrainCollider _terrainCollider;
        private float[,] _terrainHeights;
        private float[,] _preChangeTerrainHeights;

        private float _radius = 5.0f;
        private AnimationCurve _elevationCurve = new AnimationCurve();
        private ObjectRotationData _objectRotationData = new ObjectRotationData();

        private SceneOverlapFilter _sceneOverlapFilter = new SceneOverlapFilter();
        private Patch _editPatch = new Patch();
        private List<Vector3> _modelRadiusCirclePoints = new List<Vector3>();
        private List<Vector3> _radiusCirclePoints = new List<Vector3>();

        private HashSet<GameObject> _affectedObjectsSet = new HashSet<GameObject>();
        private List<TerrainGizmoAffectedObject> _affectedObjects = new List<TerrainGizmoAffectedObject>();
        private List<LocalTransformSnapshot> _preChangeTransformSnapshots;
        private Vector3 _preChangeGizmoPos;
        private List<GameObject> _objectsInRadius = new List<GameObject>();

        private GizmoLineSlider3D _axisSlider;
        private GizmoCap3D _midCap;

        private RadiusTick _leftRadiusTick;
        private RadiusTick _rightRadiusTick;
        private RadiusTick _backRadiusTick;
        private RadiusTick _forwardRadiusTick;
        private GizmoSglAxisOffsetDrag3D _radiusDrag = new GizmoSglAxisOffsetDrag3D();
        private GizmoUniformScaleDrag3D _dummyDrag = new GizmoUniformScaleDrag3D();

        private TerrainGizmoLookAndFeel _lookAndFeel = new TerrainGizmoLookAndFeel();
        private TerrainGizmoLookAndFeel _sharedLookAndFeel;

        private TerrainGizmoSettings _settings = new TerrainGizmoSettings();
        private TerrainGizmoSettings _sharedSettings;

        private TerrainGizmoHotkeys _hotkeys = new TerrainGizmoHotkeys();
        private TerrainGizmoHotkeys _sharedHotkeys;

        public TerrainGizmoLookAndFeel LookAndFeel { get { return _sharedLookAndFeel == null ? _lookAndFeel : _sharedLookAndFeel; } }
        public TerrainGizmoLookAndFeel SharedLookAndFeel
        {
            get { return _sharedLookAndFeel; }
            set
            {
                _sharedLookAndFeel = value;
                SetupSharedLookAndFeel();
            }
        }
        public TerrainGizmoSettings Settings { get { return _sharedSettings == null ? _settings : _sharedSettings; } }
        public TerrainGizmoSettings SharedSettings { get { return _sharedSettings; } set { _sharedSettings = value; } }
        public TerrainGizmoHotkeys Hotkeys { get { return _sharedHotkeys == null ? _hotkeys : _sharedHotkeys; } }
        public TerrainGizmoHotkeys SharedHotkeys { get { return _sharedHotkeys; } set { _sharedHotkeys = value; } }
        public Terrain TargetTerrain { get { return _targetTerrain; } }
        public float Radius { get { return _radius; } set { _radius = Mathf.Max(1e-4f, value); } }
        public AnimationCurve ElevationCurve { get { return _elevationCurve; } set { if (!Gizmo.IsDragged) _elevationCurve = value; } }
        public bool IsSnapEnabled { get { return _isSnapEnabled || Hotkeys.EnableSnapping.IsActive(); } }
        public bool IsRotatingObjects { get { return Hotkeys.RotateObjects.IsActive(); } }
        public TargetTypeFlags TargetTypes { get { return _targetTypeFlags; } set { if (!Gizmo.IsDragged) _targetTypeFlags = value; } }
        public bool HasTerrainTarget { get { return (int)(_targetTypeFlags & TargetTypeFlags.Terrain) != 0; } }
        public bool HasObjectsInRadiusTarget { get { return (int)(_targetTypeFlags & TargetTypeFlags.ObjectsInRadius) != 0; } }

        public void SetSnapEnabled(bool enabled)
        {
            _isSnapEnabled = enabled;
        }

        public void SetTargetTerrain(Terrain terrain)
        {
            _targetTerrain = terrain;
            if (terrain != null)
            {
                _terrainCollider = _targetTerrain.GetComponent<TerrainCollider>();
                _terrainHeights = _targetTerrain.terrainData.GetHeights(0, 0, _targetTerrain.terrainData.heightmapResolution, _targetTerrain.terrainData.heightmapResolution);
                _preChangeTerrainHeights = _terrainHeights.Clone() as float[,];
            }

            SetVisible(false);
        }

        public override void OnAttached()
        {
            _axisSlider = new GizmoLineSlider3D(Gizmo, GizmoHandleId.AxisSlider, GizmoHandleId.AxisSliderCap);
            _axisSlider.SetDragChannel(GizmoDragChannel.Offset);
            _axisSlider.MapDirection(1, AxisSign.Positive);

            _midCap = new GizmoCap3D(Gizmo, GizmoHandleId.MidSnapCap);
            _midCap.HoverPriority3D.MakeHigherThan(_axisSlider.HoverPriority3D);
            _midCap.DragSession = _dummyDrag;

            _leftRadiusTick = new RadiusTick();
            _leftRadiusTick.DragAxis = -Vector3.right;
            _leftRadiusTick.Tick = new GizmoCap2D(Gizmo, GizmoHandleId.LeftRadiusTick);
            _leftRadiusTick.Tick.DragSession = _radiusDrag;

            _rightRadiusTick = new RadiusTick();
            _rightRadiusTick.DragAxis = Vector3.right;
            _rightRadiusTick.Tick = new GizmoCap2D(Gizmo, GizmoHandleId.RightRadiusTick);
            _rightRadiusTick.Tick.DragSession = _radiusDrag;

            _backRadiusTick = new RadiusTick();
            _backRadiusTick.DragAxis = -Vector3.forward;
            _backRadiusTick.Tick = new GizmoCap2D(Gizmo, GizmoHandleId.BackRadiusTick);
            _backRadiusTick.Tick.DragSession = _radiusDrag;

            _forwardRadiusTick = new RadiusTick();
            _forwardRadiusTick.DragAxis = Vector3.forward;
            _forwardRadiusTick.Tick = new GizmoCap2D(Gizmo, GizmoHandleId.ForwardRadiusTick);
            _forwardRadiusTick.Tick.DragSession = _radiusDrag;

            const int numCirclePoints = 100;
            _modelRadiusCirclePoints = PrimitiveFactory.Generate3DCircleBorderPoints(Vector3.zero, 1.0f, Vector3.right, Vector3.forward, numCirclePoints);
            for (int ptIndex = 0; ptIndex < numCirclePoints; ++ptIndex)
                _radiusCirclePoints.Add(Vector3.zero);

            SetVisible(false);
            SetupSharedLookAndFeel();

            _sceneOverlapFilter.AllowedObjectTypes.Add(GameObjectType.Mesh);
            _sceneOverlapFilter.AllowedObjectTypes.Add(GameObjectType.Light);
            _sceneOverlapFilter.AllowedObjectTypes.Add(GameObjectType.ParticleSystem);

            _elevationCurve.AddKey(new Keyframe(0.0f, 0.0f));
            _elevationCurve.AddKey(new Keyframe(1.0f, 1.0f));

            RTUndoRedo.Get.UndoEnd += OnUndoRedoPerformed;
            RTUndoRedo.Get.RedoEnd += OnUndoRedoPerformed;
        }

        public override void OnDisabled()
        {
            RTUndoRedo.Get.UndoEnd -= OnUndoRedoPerformed;
            RTUndoRedo.Get.RedoEnd -= OnUndoRedoPerformed;
        }

        public override void OnEnabled()
        {
            RTUndoRedo.Get.UndoEnd += OnUndoRedoPerformed;
            RTUndoRedo.Get.RedoEnd += OnUndoRedoPerformed;
        }

        public override void OnGizmoUpdateBegin()
        {
            if (!IsTargetReady()) return;

            if (IsRotatingObjects)
            {
                if (!_objectRotationData.RotatingObjects)
                {
                    CollectObjectsInRadius(_objectsInRadius);
                    _objectRotationData.RotatingObjects = true;
                    _objectRotationData.GameObjects = _objectsInRadius;
                    _objectRotationData.PreSnapshots = LocalTransformSnapshot.GetSnapshotCollection(_objectRotationData.GameObjects);
                }

                float rotationAmount = RTInputDevice.Get.Device.GetFrameDelta().x * Settings.RotationSensitivity;
                Vector3 rotationAxis = _targetTerrain.transform.up;
                foreach(var go in _objectRotationData.GameObjects)
                {
                    if (CanObjectBeRotated(go))
                        go.transform.Rotate(rotationAxis, rotationAmount);
                }
            }
            else
            {
                if (_objectRotationData.RotatingObjects)
                {
                    _objectRotationData.RotatingObjects = false;

                    var action = new TerrainGizmoObjectTransformsChangedAction(_objectRotationData.PreSnapshots, LocalTransformSnapshot.GetSnapshotCollection(_objectRotationData.GameObjects));
                    action.Execute();

                    _objectRotationData.GameObjects.Clear();
                    _objectRotationData.PreSnapshots.Clear();
                }

                if (RTInputDevice.Get.Device.WasButtonPressedInCurrentFrame(0) && !Gizmo.IsHovered)
                    SnapGizmoToTerrain();
            }

            _axisSlider.Settings.OffsetSnapStep = Settings.OffsetSnapStep;
            _axisSlider.SetSnapEnabled(IsSnapEnabled);
            _radiusDrag.IsSnapEnabled = IsSnapEnabled;

            UpdateTicks();
        }

        public override bool OnGizmoCanBeginDrag(int handleId)
        {
            return IsTargetReady() && _isVisible && !IsRotatingObjects;
        }

        public override void OnGizmoAttemptHandleDragBegin(int handleId)
        {
            _affectedObjectsSet.Clear();
            _affectedObjects.Clear();

            Vector3 min = GetRadiusCircleMinExtents();
            Vector3 max = GetRadiusCircleMaxExtents();

            Vector3 rpMin = min - _targetTerrain.transform.position;
            Vector3 rpMax = max - _targetTerrain.transform.position;

            TerrainData data = _targetTerrain.terrainData;
            float uMin = rpMin.x / data.size.x;
            float uMax = rpMax.x / data.size.x;
            float vMin = rpMin.z / data.size.z;
            float vMax = rpMax.z / data.size.z;

            _editPatch.MinCol = Mathf.Clamp(Mathf.FloorToInt(uMin * data.heightmapResolution), 0, data.heightmapResolution - 1);
            _editPatch.MaxCol = Mathf.Clamp(Mathf.FloorToInt(uMax * data.heightmapResolution), 0, data.heightmapResolution - 1);
            _editPatch.MinDepth = Mathf.Clamp(Mathf.FloorToInt(vMin * data.heightmapResolution), 0, data.heightmapResolution - 1);
            _editPatch.MaxDepth = Mathf.Clamp(Mathf.FloorToInt(vMax * data.heightmapResolution), 0, data.heightmapResolution - 1);

            RadiusTick radiusTick = GetRadiusTickFromHandleId(handleId);
            if (radiusTick != null)
            {
                GizmoSglAxisOffsetDrag3D.WorkData radiusDragData = new GizmoSglAxisOffsetDrag3D.WorkData();
                radiusDragData.Axis = radiusTick.DragAxis;
                radiusDragData.DragOrigin = radiusTick.WorldPosition;
                radiusDragData.SnapStep = Settings.RadiusSnapStep;
                _radiusDrag.SetWorkData(radiusDragData);
            }

            if (handleId == _midCap.HandleId)
            {
                _preChangeGizmoPos = Gizmo.Transform.Position3D;

                GizmoUniformScaleDrag3D.WorkData dummyWorkData = new GizmoUniformScaleDrag3D.WorkData();
                dummyWorkData.CameraRight = Gizmo.GetWorkCamera().transform.right;
                dummyWorkData.CameraUp = Gizmo.GetWorkCamera().transform.up;
                dummyWorkData.DragOrigin = Gizmo.Transform.Position3D;
                dummyWorkData.SnapStep = 0.0f;
                _dummyDrag.SetWorkData(dummyWorkData);

                CollectObjectsInRadius(_objectsInRadius);
                _preChangeTransformSnapshots = LocalTransformSnapshot.GetSnapshotCollection(_objectsInRadius);
            }
        }

        public override void OnGizmoDragUpdate(int handleId)
        {
            if (handleId == _axisSlider.HandleId || handleId == _axisSlider.Cap3DHandleId)
            {
                if (HasTerrainTarget) OffsetTerrainPatch(Gizmo.RelativeDragOffset.y);
                if (HasObjectsInRadiusTarget) OffsetObjectsInRadius(Gizmo.RelativeDragOffset.y);
            }
            else
            {
                if (handleId == _midCap.HandleId)
                {
                    DragObjectsWithMidCap();
                }
                else
                {
                    RadiusTick radiusTick = GetRadiusTickFromHandleId(handleId);
                    if (radiusTick != null)
                    {
                        _radius += Vector3.Dot(Gizmo.RelativeDragOffset, radiusTick.DragAxis);
                        _radius = Mathf.Max(1e-4f, _radius);
                    }
                }
            }
        }

        public override void OnGizmoDragEnd(int handleId)
        {
            if (handleId == _midCap.HandleId)
            {
                var action = new TerrainGizmoHorizontalOffsetDragEndAction(this, _preChangeGizmoPos, 
                    _preChangeTransformSnapshots, LocalTransformSnapshot.GetSnapshotCollection(_objectsInRadius));
                action.Execute();
            }
            else
            {
                float terrainYPos = GetTerrainYPos();
                Vector3 clampedPos = Gizmo.Transform.Position3D;
                if (Gizmo.Transform.Position3D.y < terrainYPos)
                {
                    clampedPos.y = terrainYPos;
                    Gizmo.Transform.Position3D = clampedPos;
                }

                foreach (var afGo in _affectedObjects)
                    afGo.NewPosition = afGo.AffectedObject.transform.position;

                var action = new TerrainGizmoVerticalOffsetDragEndAction(_targetTerrain,
                    HasTerrainTarget ? _preChangeTerrainHeights : null, 
                    HasTerrainTarget ? _terrainHeights : null, _affectedObjects);
                action.Execute();

                Array.Copy(_terrainHeights, _preChangeTerrainHeights, _terrainHeights.GetLength(0) * _preChangeTerrainHeights.GetLength(1));
            }

            _affectedObjectsSet.Clear();
            _affectedObjects.Clear();

            ProjectGizmoOnTerrain();
        }

        public override void OnGizmoRender(Camera camera)
        {
            if (!IsTargetReady()) return;

            if (RTGizmosEngine.Get.NumRenderCameras > 1)
            {
                UpdateTicks();
            }

            if (_isVisible)
            {
                var wireMaterial = GizmoLineMaterial.Get;
                wireMaterial.ResetValuesToSensibleDefaults();
                wireMaterial.SetColor(LookAndFeel.RadiusCircleColor);
                wireMaterial.SetPass(0);

                float terrainY = _targetTerrain.transform.position.y;
                Vector3 circleCenter = Gizmo.Transform.Position3D;
                int numCirclePoints = _modelRadiusCirclePoints.Count;
                for (int ptIndex = 0; ptIndex < numCirclePoints; ++ptIndex)
                {
                    Vector3 ptPos = _modelRadiusCirclePoints[ptIndex] * _radius + circleCenter;
                    ptPos.y = terrainY + _targetTerrain.SampleHeight(_radiusCirclePoints[ptIndex]);
                    _radiusCirclePoints[ptIndex] = ptPos;
                }
                GLRenderer.DrawLines3D(_radiusCirclePoints);
            }

            _axisSlider.Render(camera);
            _midCap.Render(camera);

            _leftRadiusTick.Tick.Render(camera);
            _rightRadiusTick.Tick.Render(camera);
            _backRadiusTick.Tick.Render(camera);
            _forwardRadiusTick.Tick.Render(camera);
        }

        private bool CanObjectBeMovedHrz(GameObject go)
        {
            if (((Settings.ObjectHrzMoveLayerMask >> go.layer) & 0x1) == 0) return false;
            return !Settings.IsTagIgnoredForHrzMove(go.tag);
        }

        private bool CanObjectBeMovedVert(GameObject go)
        {
            if (((Settings.ObjectVertMoveLayerMask >> go.layer) & 0x1) == 0) return false;
            return !Settings.IsTagIgnoredForVertMove(go.tag);
        }

        private bool CanObjectBeRotated(GameObject go)
        {
            if (((Settings.ObjectRotationLayerMask >> go.layer) & 0x1) == 0) return false;
            return !Settings.IsTagIgnoredForRotation(go.tag);
        }

        private RadiusTick GetRadiusTickFromHandleId(int handleId)
        {
            RadiusTick radiusTick = null;
            if (handleId == _leftRadiusTick.Tick.HandleId) radiusTick = _leftRadiusTick;
            else if (handleId == _rightRadiusTick.Tick.HandleId) radiusTick = _rightRadiusTick;
            else if (handleId == _forwardRadiusTick.Tick.HandleId) radiusTick = _forwardRadiusTick;
            else if (handleId == _backRadiusTick.Tick.HandleId) radiusTick = _backRadiusTick;

            return radiusTick;
        }

        private void OnUndoRedoPerformed(IUndoRedoAction action)
        {
            ProjectGizmoOnTerrain();

            _terrainHeights = _targetTerrain.terrainData.GetHeights(0, 0, _targetTerrain.terrainData.heightmapResolution, _targetTerrain.terrainData.heightmapResolution);
            Array.Copy(_terrainHeights, _preChangeTerrainHeights, _terrainHeights.GetLength(0) * _preChangeTerrainHeights.GetLength(1));
        }

        private float GetTerrainYPos()
        {
            return _targetTerrain.transform.position.y;
        }

        private void ProjectGizmoOnTerrain()
        {
            Vector3 gizmoPos = Gizmo.Transform.Position3D;
            gizmoPos.y = _targetTerrain.SampleHeight(gizmoPos) + GetTerrainYPos();
            Gizmo.Transform.Position3D = gizmoPos;
        }

        private void DragObjectsWithMidCap()
        {
            RaycastHit rayHit;
            Ray ray = RTInputDevice.Get.Device.GetRay(Gizmo.GetWorkCamera());
            if (_terrainCollider.Raycast(ray, out rayHit, float.MaxValue))
            {
                Vector3 newPos = rayHit.point;
                Vector3 moveOffset = newPos - _gizmo.Transform.Position3D;
                _gizmo.Transform.Position3D += moveOffset;

                float terrainYPos = _targetTerrain.transform.position.y;
                foreach(var go in _objectsInRadius)
                {
                    if (!CanObjectBeMovedHrz(go)) continue;

                    Vector3 objectPos = go.transform.position;
                    float terrHeight = _targetTerrain.SampleHeight(objectPos);
                    float offsetFromTerrain = objectPos.y - (terrainYPos + terrHeight);

                    objectPos += moveOffset;
                    terrHeight = _targetTerrain.SampleHeight(objectPos);
                    objectPos.y = (terrainYPos + terrHeight) + offsetFromTerrain;
                    go.transform.position = objectPos;
                }
            }
        }

        private void OffsetTerrainPatch(float offset)
        {
            float terrainOffset = offset / _targetTerrain.terrainData.heightmapScale.y;
            float patchWidth = _targetTerrain.terrainData.size.x / _targetTerrain.terrainData.heightmapResolution;
            float patchDepth = _targetTerrain.terrainData.size.x / _targetTerrain.terrainData.heightmapResolution;

            Vector3 terrainPos = _targetTerrain.transform.position;
            Vector3 vertexPos = Vector3.zero;
            vertexPos.y = terrainPos.y;
            Vector3 gizmoPos = Gizmo.Transform.Position3D;
            gizmoPos.y = vertexPos.y;
            for (int depth = _editPatch.MinDepth; depth <= _editPatch.MaxDepth; ++depth)
            {
                for (int col = _editPatch.MinCol; col <= _editPatch.MaxCol; ++col)
                {
                    vertexPos.x = terrainPos.x + col * patchWidth;
                    vertexPos.z = terrainPos.z + depth * patchDepth;

                    float d = (vertexPos - gizmoPos).magnitude;
                    if (d <= _radius)
                    {
                        float t = Mathf.Max(1.0f - d / _radius, 0.0f);
                        t = _elevationCurve.Evaluate(t);

                        _terrainHeights[depth, col] += terrainOffset * t;
                        _terrainHeights[depth, col] = Mathf.Max(_terrainHeights[depth, col], 0.0f);
                    }
                }
            }

            _targetTerrain.terrainData.SetHeights(0, 0, _terrainHeights);
        }

        private void OffsetObjectsInRadius(float offset)
        {
            float terrainYPos = GetTerrainYPos();
            Vector3 gizmoPos = Gizmo.Transform.Position3D;
            //var overlappedObject = CollectObjectsInRadius();
            if (_objectsInRadius.Count != 0)
            {
                gizmoPos.y = terrainYPos;
                foreach (var go in _objectsInRadius)
                {
                    if (!CanObjectBeMovedVert(go)) continue;

                    Transform transform = go.transform;
                    Vector3 objectPos = transform.position;
                    objectPos.y = terrainYPos;

                    float d = (objectPos - gizmoPos).magnitude;
                    if (d <= _radius)
                    {
                        float t = Mathf.Max(1.0f - d / _radius, 0.0f);
                        t = _elevationCurve.Evaluate(t);

                        Vector3 newPos = transform.position + Vector3.up * offset * t;
                        if (newPos.y < terrainYPos) newPos.y = terrainYPos;

                        if (!_affectedObjectsSet.Contains(go))
                        {
                            _affectedObjectsSet.Add(go);
                            _affectedObjects.Add(new TerrainGizmoAffectedObject() { AffectedObject = go, OriginalObjectPos = transform.position });
                        }

                        transform.position = newPos;
                    }
                }
            }
        }

        private List<GameObject> _objectCollectRadius = new List<GameObject>();
        private void CollectObjectsInRadius(List<GameObject> objectsInRadius)
        {
            objectsInRadius.Clear();

            OBB overlapBox = new OBB();
            overlapBox.Size = (new Vector3(_radius, _targetTerrain.terrainData.size.y, _radius)) * 2.0f;
            var boxCenter = Gizmo.Transform.Position3D;
            overlapBox.Center = boxCenter;

            RTScene.Get.OverlapBox(overlapBox, _sceneOverlapFilter, _objectCollectRadius);
            GameObjectEx.FilterParentsOnly(_objectCollectRadius, objectsInRadius);
            objectsInRadius.RemoveAll(item => !IsObjectInRadius(item));
        }

        private bool IsObjectInRadius(GameObject gameObject)
        {
            Vector3 gizmoPos = Gizmo.Transform.Position3D;
            gizmoPos.y = 0.0f;
            Vector3 objectPos = gameObject.transform.position;
            objectPos.y = 0.0f;

            return (gizmoPos - objectPos).magnitude <= _radius;
        }

        private void UpdateTicks()
        {
            Camera camera = Gizmo.GetWorkCamera();

            float terrainYPos = GetTerrainYPos();
            Vector3 gizmoPos = Gizmo.Transform.Position3D;
            Vector3 tickWorldPos = gizmoPos - Vector3.right * _radius;
            tickWorldPos.y = terrainYPos + _targetTerrain.SampleHeight(tickWorldPos);
            _leftRadiusTick.Tick.Position = camera.WorldToScreenPoint(tickWorldPos);
            _leftRadiusTick.WorldPosition = tickWorldPos;

            tickWorldPos = gizmoPos + Vector3.right * _radius;
            tickWorldPos.y = terrainYPos + _targetTerrain.SampleHeight(tickWorldPos);
            _rightRadiusTick.Tick.Position = camera.WorldToScreenPoint(tickWorldPos);
            _rightRadiusTick.WorldPosition = tickWorldPos;

            tickWorldPos = gizmoPos - Vector3.forward * _radius;
            tickWorldPos.y = terrainYPos + _targetTerrain.SampleHeight(tickWorldPos);
            _backRadiusTick.Tick.Position = camera.WorldToScreenPoint(tickWorldPos);
            _backRadiusTick.WorldPosition = tickWorldPos;

            tickWorldPos = gizmoPos + Vector3.forward * _radius;
            tickWorldPos.y = terrainYPos + _targetTerrain.SampleHeight(tickWorldPos);
            _forwardRadiusTick.Tick.Position = camera.WorldToScreenPoint(tickWorldPos);
            _forwardRadiusTick.WorldPosition = tickWorldPos;
        }

        private Vector3 GetRadiusCircleMinExtents()
        {
            return Gizmo.Transform.Position3D - Vector3.right * _radius - Vector3.forward * _radius;
        }

        private Vector3 GetRadiusCircleMaxExtents()
        {
            return Gizmo.Transform.Position3D + Vector3.right * _radius + Vector3.forward * _radius;
        }

        private void SnapGizmoToTerrain()
        {
            RaycastHit rayHit;
            Ray ray = RTInputDevice.Get.Device.GetRay(Gizmo.GetWorkCamera());
            if (_terrainCollider.Raycast(ray, out rayHit, float.MaxValue))
            {
                _gizmo.Transform.Position3D = rayHit.point;
                SetVisible(true);
            }
        }

        private bool IsTargetReady()
        {
            return _targetTerrain != null && _terrainCollider != null;
        }

        private void SetupSharedLookAndFeel()
        {
            LookAndFeel.ConnectAxisSliderLookAndFeel(_axisSlider);
            LookAndFeel.ConnectMidCapLookAndFeel(_midCap);
            LookAndFeel.ConnectRadiusTickLookAndFeel(_leftRadiusTick.Tick);
            LookAndFeel.ConnectRadiusTickLookAndFeel(_rightRadiusTick.Tick);
            LookAndFeel.ConnectRadiusTickLookAndFeel(_backRadiusTick.Tick);
            LookAndFeel.ConnectRadiusTickLookAndFeel(_forwardRadiusTick.Tick);
        }

        private void SetVisible(bool visible)
        {
            _axisSlider.SetVisible(visible);
            _axisSlider.Set3DCapVisible(visible);
            _midCap.SetVisible(visible);

            _leftRadiusTick.Tick.SetVisible(visible);
            _rightRadiusTick.Tick.SetVisible(visible);
            _backRadiusTick.Tick.SetVisible(visible);
            _forwardRadiusTick.Tick.SetVisible(visible);

            _isVisible = visible;
        }
    }
}
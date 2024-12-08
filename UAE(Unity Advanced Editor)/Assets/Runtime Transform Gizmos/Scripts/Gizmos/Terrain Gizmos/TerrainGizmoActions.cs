using UnityEngine;
using System.Collections.Generic;

namespace RTG
{
    public class TerrainGizmoObjectTransformsChangedAction : IUndoRedoAction
    {
        private List<LocalTransformSnapshot> _preChangeTransformSnapshots = new List<LocalTransformSnapshot>();
        private List<LocalTransformSnapshot> _postChangeTransformSnapshots = new List<LocalTransformSnapshot>();

        public TerrainGizmoObjectTransformsChangedAction(List<LocalTransformSnapshot> preChangeTransformSnapshots,
                                                         List<LocalTransformSnapshot> postChangeTransformSnapshots)
        {
            _preChangeTransformSnapshots = new List<LocalTransformSnapshot>(preChangeTransformSnapshots);
            _postChangeTransformSnapshots = new List<LocalTransformSnapshot>(postChangeTransformSnapshots);
        }

        public void Execute()
        {
            RTUndoRedo.Get.RecordAction(this);
        }

        public void Undo()
        {
            foreach (var snapshot in _preChangeTransformSnapshots)
            {
                snapshot.Apply();
            }
        }

        public void Redo()
        {
            foreach (var snapshot in _postChangeTransformSnapshots)
            {
                snapshot.Apply();
            }
        }

        public void OnRemovedFromUndoRedoStack()
        {
        }
    }

    public class TerrainGizmoHorizontalOffsetDragEndAction : IUndoRedoAction
    {
        private List<LocalTransformSnapshot> _preChangeSnapshots = new List<LocalTransformSnapshot>();
        private List<LocalTransformSnapshot> _postChangeSnapshots = new List<LocalTransformSnapshot>();
        private Vector3 _preChangeGizmoPos;
        private Vector3 _postChangeGizmoPos;
        private TerrainGizmo _terrainGizmo;

        public TerrainGizmoHorizontalOffsetDragEndAction(TerrainGizmo terrainGizmo, Vector3 preChangeGizmoPos, 
            List<LocalTransformSnapshot> preChangeSnapshots, List<LocalTransformSnapshot> postChangeSnapshots)
        {
            _preChangeSnapshots = new List<LocalTransformSnapshot>(preChangeSnapshots);
            _postChangeSnapshots = new List<LocalTransformSnapshot>(postChangeSnapshots);
            _preChangeGizmoPos = preChangeGizmoPos;
            _postChangeGizmoPos = terrainGizmo.Gizmo.Transform.Position3D;
            _terrainGizmo = terrainGizmo;
        }

        public void Execute()
        {
            RTUndoRedo.Get.RecordAction(this);
        }

        public void OnRemovedFromUndoRedoStack()
        {
        }

        public void Redo()
        {
            _terrainGizmo.Gizmo.Transform.Position3D = _postChangeGizmoPos;
            foreach(var snapshot in _postChangeSnapshots)
            {
                snapshot.Apply();
            }
        }

        public void Undo()
        {
            _terrainGizmo.Gizmo.Transform.Position3D = _preChangeGizmoPos;
            foreach (var snapshot in _preChangeSnapshots)
            {
                snapshot.Apply();
            }
        }
    }

    public class TerrainGizmoVerticalOffsetDragEndAction : IUndoRedoAction
    {
        private List<TerrainGizmoAffectedObject> _affectedObjects;
        private Terrain _terrain;
        private float[,] _preChangeHeights;
        private float[,] _postChangeHeights;

        public TerrainGizmoVerticalOffsetDragEndAction(Terrain terrain, float[,] preChangeHeights, 
            float[,] postChangeHeights, List<TerrainGizmoAffectedObject> affectedObjects)
        {
            _terrain = terrain;
            if (preChangeHeights != null) _preChangeHeights = preChangeHeights.Clone() as float[,];
            if (postChangeHeights != null) _postChangeHeights = postChangeHeights.Clone() as float[,];
            _affectedObjects = new List<TerrainGizmoAffectedObject>(affectedObjects);
        }

        public void Execute()
        {
            RTUndoRedo.Get.RecordAction(this);
        }

        public void OnRemovedFromUndoRedoStack()
        {
        }

        public void Redo()
        {
            if (_terrain != null && _postChangeHeights != null)
                _terrain.terrainData.SetHeights(0, 0, _postChangeHeights);

            foreach (var afGo in _affectedObjects)
            {
                if (afGo.AffectedObject != null)
                    afGo.AffectedObject.transform.position = afGo.NewPosition;
            }
        }

        public void Undo()
        {
            if (_terrain != null && _preChangeHeights != null)
                _terrain.terrainData.SetHeights(0, 0, _preChangeHeights);

            foreach (var afGo in _affectedObjects)
            {
                if (afGo.AffectedObject != null)
                    afGo.AffectedObject.transform.position = afGo.OriginalObjectPos;
            }
        }
    }
}

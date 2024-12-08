using UnityEngine;

namespace RTG
{
    public class BoxCollider3DSnapshot
    {
        private BoxCollider _boxCollider;
        private Vector3 _localCenter;
        private Vector3 _localSize;

        public BoxCollider3DSnapshot() { }
        public BoxCollider3DSnapshot(BoxCollider3DSnapshot src)
        {
            _boxCollider = src._boxCollider;
            _localCenter = src._localCenter;
            _localSize = src._localSize;
        }

        public void Snapshot(BoxCollider boxCollider)
        {
            if (boxCollider == null) return;

            _boxCollider = boxCollider;
            _localCenter = boxCollider.center;
            _localSize = boxCollider.size;
        }

        public void Apply()
        {
            if (_boxCollider == null) return;

            _boxCollider.center = _localCenter;
            _boxCollider.size = _localSize;
        }
    }
}


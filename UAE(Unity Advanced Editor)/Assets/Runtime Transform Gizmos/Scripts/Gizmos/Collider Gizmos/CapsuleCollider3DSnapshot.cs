using UnityEngine;

namespace RTG
{
    public class CapsuleCollider3DSnapshot
    {
        private CapsuleCollider _capsuleCollider;
        private Vector3 _localCenter;
        private float _localRadius;
        private float _localHeight;

        public CapsuleCollider3DSnapshot() { }
        public CapsuleCollider3DSnapshot(CapsuleCollider3DSnapshot src)
        {
            _capsuleCollider = src._capsuleCollider;
            _localCenter = src._localCenter;
            _localRadius = src._localRadius;
            _localHeight = src._localHeight;
        }

        public void Snapshot(CapsuleCollider capsuleCollider)
        {
            if (capsuleCollider == null) return;

            _capsuleCollider = capsuleCollider;
            _localCenter = capsuleCollider.center;
            _localRadius = capsuleCollider.radius;
            _localHeight = capsuleCollider.height;
        }

        public void Apply()
        {
            if (_capsuleCollider == null) return;

            _capsuleCollider.center = _localCenter;
            _capsuleCollider.radius = _localRadius;
            _capsuleCollider.height = _localHeight;
        }
    }
}
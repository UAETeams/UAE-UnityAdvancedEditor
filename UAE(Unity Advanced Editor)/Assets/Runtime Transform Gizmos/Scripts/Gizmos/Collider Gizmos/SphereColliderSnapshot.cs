using UnityEngine;

namespace RTG
{
    public class SphereColliderSnapshot
    {
        private SphereCollider _sphereCollider;
        private Vector3 _localCenter;
        private float _localRadius;

        public SphereColliderSnapshot() { }
        public SphereColliderSnapshot(SphereColliderSnapshot src)
        {
            _sphereCollider = src._sphereCollider;
            _localCenter = src._localCenter;
            _localRadius = src._localRadius;
        }

        public void Snapshot(SphereCollider sphereCollider)
        {
            if (sphereCollider == null) return;

            _sphereCollider = sphereCollider;
            _localCenter = sphereCollider.center;
            _localRadius = sphereCollider.radius;
        }

        public void Apply()
        {
            if (_sphereCollider == null) return;

            _sphereCollider.center = _localCenter;
            _sphereCollider.radius = _localRadius;
        }
    }
}


using UnityEngine;
using System;

namespace RTG
{
    [Serializable]
    public class SphereColliderGizmoSettings
    {
        [SerializeField]
        private float _radiusSnapStep = DefaultRadiusSnapStep;

        public static float DefaultRadiusSnapStep { get { return 0.1f; } }

        public float RadiusSnapStep { get { return _radiusSnapStep; } }

        public void SetRadiusSnapStep(float snapStep)
        {
            _radiusSnapStep = Mathf.Max(-1e-4f, snapStep);
        }
    }
}
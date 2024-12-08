using UnityEngine;
using System;

namespace RTG
{
    [Serializable]
    public class SpotLightGizmo3DSettings
    {
        [SerializeField]
        private float _radiusSnapStep = DefaultRadiusSnapStep;
        [SerializeField]
        private float _rangeSnapStep = DefaultRangeSnapStep;

        public static float DefaultRadiusSnapStep { get { return 0.1f; } }
        public static float DefaultRangeSnapStep { get { return 0.1f; } }

        public float RadiusSnapStep { get { return _radiusSnapStep; } }
        public float RangeSnapStep { get { return _rangeSnapStep; } }

        public void SetRadiusSnapStep(float snapStep)
        {
            _radiusSnapStep = Mathf.Max(1e-4f, snapStep);
        }

        public void SetRangeSnapStep(float snapStep)
        {
            _rangeSnapStep = Mathf.Max(1e-4f, snapStep);
        }
    }
}
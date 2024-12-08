using UnityEngine;
using System;

namespace RTG
{
    [Serializable]
    public class CapsuleColliderGizmo3DSettings
    {
        [SerializeField]
        private float _radiusSnapStep = DefaultRadiusSnapStep;
        [SerializeField]
        private float _heightSnapStep = DefaultHeightSnapStep;

        public static float DefaultRadiusSnapStep { get { return 0.1f; } }
        public static float DefaultHeightSnapStep { get { return 0.1f; } }

        public float RadiusSnapStep { get { return _radiusSnapStep; } }
        public float HeightSnapStep { get { return _heightSnapStep; } }

        public void SetRadiusSnapStep(float snapStep)
        {
            _radiusSnapStep = Mathf.Max(-1e-4f, snapStep);
        }

        public void SetHeightSnapStep(float snapStep)
        {
            _heightSnapStep = Mathf.Max(1e-4f, snapStep);
        }
    }
}
using UnityEngine;
using System;

namespace RTG
{
    [Serializable]
    public class BoxColliderGizmo3DSettings
    {
        [SerializeField]
        private float _xSizeSnapStep = DefaultSizeSnapStep;
        [SerializeField]
        private float _ySizeSnapStep = DefaultSizeSnapStep;
        [SerializeField]
        private float _zSizeSnapStep = DefaultSizeSnapStep;
        [SerializeField]
        private float _uniformSizeSnapStep = DefaultUniformSizeSnapStep;

        public static float DefaultSizeSnapStep { get { return 0.1f; } }
        public static float DefaultUniformSizeSnapStep { get { return 0.1f; } }

        public float XSizeSnapStep { get { return _xSizeSnapStep; } }
        public float YSizeSnapStep { get { return _ySizeSnapStep; } }
        public float ZSizeSnapStep { get { return _zSizeSnapStep; } }
        public float UniformSizeSnapStep { get { return _uniformSizeSnapStep; } }

        public void SetXSizeSnapStep(float snapStep)
        {
            _xSizeSnapStep = Mathf.Max(-1e-4f, snapStep);
        }

        public void SetYSizeSnapStep(float snapStep)
        {
            _ySizeSnapStep = Mathf.Max(-1e-4f, snapStep);
        }

        public void SetZSizeSnapStep(float snapStep)
        {
            _zSizeSnapStep = Mathf.Max(-1e-4f, snapStep);
        }

        public void SetUniformSizeSnapStep(float snapStep)
        {
            _uniformSizeSnapStep = Mathf.Max(1e-4f, snapStep);
        }
    }
}
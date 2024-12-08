using UnityEngine;
using System;
using System.Collections.Generic;

namespace RTG
{
    [Serializable]
    public class TerrainGizmoSettings
    {
        [SerializeField]
        private float _offsetSnapStep = 1.0f;
        [SerializeField]
        private float _radiusSnapStep = 1.0f;
        [SerializeField]
        private float _rotationSensitivity = 1.0f;
        [SerializeField]
        private int _objectHrzMoveLayerMask = ~0;
        [SerializeField]
        private int _objectVertMoveLayerMask = ~0;
        [SerializeField]
        private int _objectRotationLayerMask = ~0;
        [SerializeField]
        private List<string> _objectHrzMoveIgnoreTags = new List<string>();
        [SerializeField]
        private List<string> _objectVertMoveIgnoreTags = new List<string>();
        [SerializeField]
        private List<string> _objectRotationIgnoreTags = new List<string>();

        public float OffsetSnapStep { get { return _offsetSnapStep; } set { _offsetSnapStep = Mathf.Max(1e-4f, value); } }
        public float RadiusSnapStep { get { return _radiusSnapStep; } set { _radiusSnapStep = Mathf.Max(1e-4f, value); } }
        public float RotationSensitivity { get { return _rotationSensitivity; } set { _rotationSensitivity = Math.Max(1e-4f, value); } }
        public int ObjectHrzMoveLayerMask { get { return _objectHrzMoveLayerMask; } set { _objectHrzMoveLayerMask = value; } }
        public int ObjectVertMoveLayerMask { get { return _objectVertMoveLayerMask; } set { _objectVertMoveLayerMask = value; } }
        public int ObjectRotationLayerMask { get { return _objectRotationLayerMask; } set { _objectRotationLayerMask = value; } }

        public void AddObjectHrzMoveIgnoreTag(string tag)
        {
            if (!IsTagIgnoredForHrzMove(tag))
            {
                _objectHrzMoveIgnoreTags.Add(tag);
            }
        }

        public bool IsTagIgnoredForHrzMove(string tag)
        {
            return _objectHrzMoveIgnoreTags.Contains(tag);
        }

        public void AddObjectVertMoveIgnoreTag(string tag)
        {
            if (!IsTagIgnoredForVertMove(tag))
            {
                _objectVertMoveIgnoreTags.Add(tag);
            }
        }

        public bool IsTagIgnoredForVertMove(string tag)
        {
            return _objectVertMoveIgnoreTags.Contains(tag);
        }

        public void AddObjectRotationIgnoreTag(string tag)
        {
            if (!IsTagIgnoredForRotation(tag))
            {
                _objectRotationIgnoreTags.Add(tag);
            }
        }

        public bool IsTagIgnoredForRotation(string tag)
        {
            return _objectRotationIgnoreTags.Contains(tag);
        }
    }
}
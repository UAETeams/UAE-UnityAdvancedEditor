using UnityEngine;

namespace RTG
{
    public class Light3DSnapshot
    {
        private Light _light;
        private Vector3 _position;
        private Quaternion _rotation;
        private float _range;
        private float _spotAngle;

        public Light3DSnapshot() { }
        public Light3DSnapshot(Light3DSnapshot src)
        {
            _light = src._light;
            _position = src._position;
            _rotation = src._rotation;
            _range = src._range;
            _spotAngle = src._spotAngle;
        }

        public void Snapshot(Light light)
        {
            if (light == null) return;

            _light = light;
            _position = light.transform.position;
            _rotation = light.transform.rotation;
            _range = light.range;
            _spotAngle = light.spotAngle;
        }

        public void Apply()
        {
            if (_light == null) return;

            _light.transform.position = _position;
            _light.transform.rotation = _rotation;
            _light.range = _range;
            _light.spotAngle = _spotAngle;
        }
    }
}
using UnityEngine;

namespace RTG
{
    public class CharacterController3DSnapshot
    {
        private CharacterController _characterController;
        private Vector3 _localCenter;
        private float _localRadius;
        private float _localHeight;

        public CharacterController3DSnapshot() { }
        public CharacterController3DSnapshot(CharacterController3DSnapshot src)
        {
            _characterController = src._characterController;
            _localCenter = src._localCenter;
            _localRadius = src._localRadius;
            _localHeight = src._localHeight;
        }

        public void Snapshot(CharacterController characterController)
        {
            if (characterController == null) return;

            _characterController = characterController;
            _localCenter = characterController.center;
            _localRadius = characterController.radius;
            _localHeight = characterController.height;
        }

        public void Apply()
        {
            if (_characterController == null) return;

            _characterController.center = _localCenter;
            _characterController.radius = _localRadius;
            _characterController.height = _localHeight;
        }
    }
}
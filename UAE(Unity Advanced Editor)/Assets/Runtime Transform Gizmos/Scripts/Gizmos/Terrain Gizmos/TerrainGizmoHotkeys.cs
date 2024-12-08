using UnityEngine;
using System;

namespace RTG
{
    [Serializable]
    public class TerrainGizmoHotkeys
    {
        [SerializeField]
        private Hotkeys _enableSnapping = new Hotkeys("Enable snapping", new HotkeysStaticData { CanHaveMouseButtons = false })
        {
            Key = KeyCode.None,
            LCtrl = true
        };
        private Hotkeys _rotateObjects = new Hotkeys("Enable object rotation", new HotkeysStaticData { CanHaveMouseButtons = false })
        {
            Key = KeyCode.C
        };

        public Hotkeys EnableSnapping { get { return _enableSnapping; } }
        public Hotkeys RotateObjects { get { return _rotateObjects; } }
    }
}
using UnityEngine;
using System;

namespace RTG
{
    [Serializable]
    public class CharacterControllerGizmo3DHotkeys
    {
        [SerializeField]
        private Hotkeys _enableSnapping = new Hotkeys("Enable snapping", new HotkeysStaticData { CanHaveMouseButtons = false })
        {
            Key = KeyCode.None,
            LCtrl = true
        };

        [SerializeField]
        private Hotkeys _scaleFromCenter = new Hotkeys("Scale from Center", new HotkeysStaticData { CanHaveMouseButtons = false })
        {
            Key = KeyCode.None,
            LShift = true
        };

        public Hotkeys EnableSnapping { get { return _enableSnapping; } }
        public Hotkeys ScaleFromCenter { get { return _scaleFromCenter; } }
    }
}

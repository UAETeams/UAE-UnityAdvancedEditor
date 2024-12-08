using UnityEngine;
using System;

namespace RTG
{
    [Serializable]
    public class SpotLightGizmo3DHotkeys
    {
        [SerializeField]
        private Hotkeys _enableSnapping = new Hotkeys("Enable snapping", new HotkeysStaticData { CanHaveMouseButtons = false })
        {
            Key = KeyCode.None,
            LCtrl = true
        };

        public Hotkeys EnableSnapping { get { return _enableSnapping; } }
    }
}

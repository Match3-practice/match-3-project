using UnityEngine;

namespace GUI.MainMenu
{

    public class SettingsPanel : MonoBehaviour
    {
        [HideInInspector]
        public Vector3 OriginalPosition = Vector3.zero;

        public bool IsActive { get; set; } = false;

        private void Awake()
        {
            OriginalPosition = transform.position;
        }

        public void SetPanelActive(bool value)
        {
            IsActive = value;
        }
    }
}

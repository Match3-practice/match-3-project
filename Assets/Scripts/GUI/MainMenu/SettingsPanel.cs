using UnityEngine;

namespace GUI.MainMenu
{
    public class SettingsPanel : MonoBehaviour
    {
        public Vector3 OriginalPosition = Vector3.zero;
        public bool IsActive = false;

        private void Awake()
        {
            OriginalPosition = transform.position;
        }
    }
}

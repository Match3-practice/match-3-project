using DG.Tweening;
using UnityEngine;

namespace GUI.MainMenu
{
    public class HideSettingsButton : MonoBehaviour
    {
        [SerializeField] private SettingsPanel Panel = null;
        [SerializeField] private Transform TargetPosition = null;

        private float Duration = 1f;


        public void OnButtonClick()
        {
            ButtonClickSequence();
        }

        private void ButtonClickSequence()
        {
            DOTween.Sequence()
                .Append(transform.DOScale(0.75f, 0.5f))
                .Append(transform.DOScale(1f, 0.5f))
                .AppendInterval(0.3f)
                .OnComplete(HideSettings);
        }

        private void HideSettings()
        {
            if (Panel.IsActive != false && Panel != null)
            {
                Panel.transform.DOMove(Panel.OriginalPosition, Duration);
                Panel.IsActive = false;
            }
        }
    }
}
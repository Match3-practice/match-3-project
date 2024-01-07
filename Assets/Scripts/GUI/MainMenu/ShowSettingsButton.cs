using DG.Tweening;
using UnityEngine;

namespace GUI.MainMenu
{
    public class ShowSettingsButton : MonoBehaviour
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
                .OnComplete(ShowSettings);
        }

        private void ShowSettings()
        {
            if (Panel.IsActive != true && Panel != null)
            {
                Panel.transform.DOMove(TargetPosition.position, Duration);
                Panel.IsActive = true;
            }
        }
    }
}

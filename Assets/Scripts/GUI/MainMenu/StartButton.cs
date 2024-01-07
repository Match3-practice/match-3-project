using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GUI.MainMenu
{
    public class StartButton : MonoBehaviour
    {
        public void OnButtonClick()
        {
            DOTween.Sequence()
                .Append(transform.DOScale(0.75f, 0.5f))
                .Append(transform.DOScale(1f, 0.5f))
                .AppendInterval(0.3f)
                .OnComplete(LoadScene);
        }

        private void LoadScene()
        {
            SceneManager.LoadScene("Scenes/GameplayScene(Lv0)");
        }
    }
}

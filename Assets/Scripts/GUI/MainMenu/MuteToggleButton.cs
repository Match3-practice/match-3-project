using System;
using UnityEngine;
using UnityEngine.UI;

public class MuteToggleButton : MonoBehaviour
{
    [SerializeField] private Sprite[] sprite = null;

    private bool isMute = false;

    public bool IsMute
    {
        get { return isMute; }
        set
        {
            isMute = value;

            ToggleSound();
        }
    }

    private void ToggleSound()
    {
        //throw new NotImplementedException("Need implement toggle func");
    }

    public void OnButtonClick()
    {
        IsMute = StateToggle();
    }

    private bool StateToggle()
    {
        if (isMute != false)
        {
            return ChangeSprite(false);
        }
        else
        {
            return ChangeSprite(true);
        }
    }

    private bool ChangeSprite(bool state)
    {
        if (sprite != null)
        {
            int spriteIndex = state ? 1 : 0;

            gameObject.GetComponent<Image>().sprite = sprite[spriteIndex];
            return state;
        }
        else throw new NullReferenceException($"Missing sprites in {gameObject.name}");
    }
}

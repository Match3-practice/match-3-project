using DG.Tweening;
using System;
using UnityEngine;

public class DOTweenCrystalAnimService : ICrystalAnimationService
{
    public Tween Tween {get; set;}  

    public void AnimatePosition(GameObject crystal, Vector3 targetPos, float speed = 1f)
    {
        if (Tween != null)
        {
            Tween.Kill();
            Tween = null;
        }

        Tween = crystal.transform.DOMove(targetPos, speed);

        Sequence sequence = DOTween.Sequence();


    }

    public void AnimatePosition(GameObject crystal, Transform targetTransform, float speed = 1)
    {
        crystal.transform.DOMove(targetTransform.position, speed);
    }
    public void AnimateDestroy(GameObject crystal, Action action = null, float duration = 5f)
    {
        if(action != null)
        crystal.transform.DOLocalMove(new Vector3(0, -5000f, 0), duration).SetEase(Ease.InBack).OnComplete(()=> action());
        else
        crystal.transform.DOLocalMove(new Vector3(0, -5000f, 0), duration).SetEase(Ease.InBack);
    }

    public Tween GetTween(string name)
    {
        throw new NotImplementedException();
    }
}
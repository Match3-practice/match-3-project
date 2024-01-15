using DG.Tweening;
using System;
using UnityEngine;

public class DOTweenCrystalAnimService : ICrystalAnimationService
{
    private Tween tween = null;

    public void AnimatePosition(GameObject crystal, Vector3 targetPos, float speed = 1)
    {
        if (tween != null)
        {
            tween.Kill();
            tween = null;
        }

        tween = crystal.transform.DOMove(targetPos, speed);

    }

    public void AnimatePosition(GameObject crystal, Transform targetTransform, float speed = 1)
    {
        crystal.transform.DOMove(targetTransform.position, speed);
    }
    public void AnimateDestroy(GameObject crystal, Action action = null, float duration = 5f)
    {
        if(action != null)
        crystal.transform.DOLocalMove(new Vector3(0, -5000f, 0), duration).OnComplete(()=> action());
        else
        crystal.transform.DOLocalMove(new Vector3(0, -5000f, 0), duration);
    }
}
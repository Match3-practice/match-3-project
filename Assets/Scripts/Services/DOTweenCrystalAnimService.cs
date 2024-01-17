using DG.Tweening;
using System;
using UnityEngine;

public class DOTweenCrystalAnimService : ICrystalAnimationService
{
    public Tween Tween { get; set; }
    public static Sequence sequence = DOTween.Sequence();
    public static bool IsAnimated = false;
    //public void AnimatePosition(GameObject crystal, Vector3 targetPos, float speed = 1f)
    //{
    //    if (Tween != null)
    //    {
    //        Tween.Kill();
    //        Tween = null;
    //    }

    //    Tween = crystal.transform.DOMove(targetPos, speed);

    //    Sequence sequence = DOTween.Sequence();
    //    sequence.Pause();
    //    sequence.Append(crystal.transform.DOMove(targetPos, speed))
    //        .Join(crystal.transform.DOMove(targetPos, speed));


    //}
    
    public static void EndAnimations()
    {
        IsAnimated = false;
    }
    public void AnimatePosition(GameObject crystal, Transform targetTransform, float speed = 1f)
    {
        if (IsAnimated)
            sequence.Join(crystal.transform.DOMove(targetTransform.position, speed));
        else
        {
            IsAnimated = true;
            if (sequence == null || !sequence.active)
                sequence = DOTween.Sequence();
            sequence.Append(crystal.transform.DOMove(targetTransform.position, speed));
        }
    }
    public void AnimateDestroy(GameObject crystal, Action action = null, float duration = 1f)
    {
        Tween animated = crystal.transform.DOLocalMove(new Vector3(0, -5000f, 0), duration).SetEase(Ease.InBack);
        if (action != null)
        {
            if (IsAnimated)
                sequence.Join(animated.OnComplete(() => action()));
            else
                sequence.Append(animated.OnComplete(() => action()));
        }
        else
        {
            if (IsAnimated)
                sequence.Join(animated);
            else
                sequence.Append(animated);
        }
        IsAnimated = true;
    }

    public Tween GetTween(string name)
    {
        throw new NotImplementedException();
    }
}
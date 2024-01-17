using DG.Tweening;
using System;
using UnityEngine;

public class DOTweenCrystalAnimService : ICrystalAnimationService
{
    public Tween Tween { get; set; }
    public static Sequence sequence = DOTween.Sequence();

    public void AnimatePosition(GameObject crystal, Vector3 targetPos, float speed = 1f)
    {
        if (Tween != null)
        {
            Tween.Kill();
            Tween = null;
        }

        Tween = crystal.transform.DOMove(targetPos, speed);

        Sequence sequence = DOTween.Sequence();
        sequence.Pause();
        sequence.Append(crystal.transform.DOMove(targetPos, speed))
            .Join(crystal.transform.DOMove(targetPos, speed));


    }
    //public void AddAnimationToSequence(GameObject crystal, Vector3 targetPos, float speed = 1f)
    //{
    //    if (sequence == null)
    //        sequence = DOTween.Sequence();
    //    if (sequence.Duration() == 0)
    //        sequence.Append(crystal.transform.DOMove(targetPos, speed));
    //    else
    //        sequence.Join(crystal.transform.DOMove(targetPos, speed));
    //}
    //public void AddAnimationToSequence(Tween animation)
    //{
    //    if (sequence == null)
    //        sequence = DOTween.Sequence();
    //    if (sequence.Duration() == 0)
    //        sequence.Append(animation);
    //    else
    //        sequence.Join(animation);
    //}
    public static void PlayAnimation()
    {
        if (sequence != null)
        {
            sequence.Play();
            sequence = null;
        }
    }
    public void AnimatePosition(GameObject crystal, Transform targetTransform, float speed = 1)
    {
        if (sequence.Duration() == 0)
            sequence.Append(crystal.transform.DOMove(targetTransform.position, speed));
        else
            sequence.Join(crystal.transform.DOMove(targetTransform.position, speed));
    }
    public void AnimateDestroy(GameObject crystal, Action action = null, float duration = 5f)
    {
        if (action != null)
        {
            if (sequence.Duration() == 0)
                sequence.Append(crystal.transform.DOLocalMove(new Vector3(0, -5000f, 0), duration).SetEase(Ease.InBack).OnComplete(() => action()));
            else
                sequence.Join(crystal.transform.DOLocalMove(new Vector3(0, -5000f, 0), duration).SetEase(Ease.InBack).OnComplete(() => action()));
        }
        else
        {
            if (sequence.Duration() == 0)
                sequence.Append(crystal.transform.DOLocalMove(new Vector3(0, -5000f, 0), duration).SetEase(Ease.InBack));
            else
                sequence.Join(crystal.transform.DOLocalMove(new Vector3(0, -5000f, 0), duration).SetEase(Ease.InBack));



                sequence
        }
    }

    public Tween GetTween(string name)
    {
        throw new NotImplementedException();
    }
}
using DG.Tweening;
using System;
using UnityEngine;

public static class DOTweenCrystalAnimService
{
    public static Sequence sequence = DOTween.Sequence();
    public static bool IsAnimated = false;
    
    public static void EndAnimations()
    {
        IsAnimated = false;
    }

    #region MetodDescription
    ///<summary>
    ///Animate crystal movement from current to target position
    ///</summary>
    ///<remarks> 
    ///!!!WARNING!!! Animations work asynchronously relative to the main thread.
    /// You must wait until the animation finishes for the animation to play correctly.
    /// <list type="table">
    /// <item>
    /// <term>crystal</term>
    /// <description>GameObject which will be animated</description>
    /// </item>
    /// <item>
    /// <term>targetTransform</term>
    /// <description>
    /// Target position, implicitly converts Transform to Vecrtor3
    /// </description>
    /// </item>
    /// <item>
    /// <term>speed</term>
    /// <description> Speed of animation. Default = 1f </description>
    ///</item>
    /// </list>
    /// </remarks>
    ///<param name="speed">Animation duration. Default = 1f </param>
    #endregion
    public static void AnimatePosition(GameObject crystal, Transform targetTransform, float speed = 1f)
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
    public static void AnimateMoveFromPointToPoint(GameObject crystal, Vector3 startTransform, Vector3 endTransform, float speed = 1f)
    {
        if (IsAnimated)
            sequence.Join(crystal.transform.DOMove(endTransform, speed).From(startTransform));
        else
        {
            IsAnimated = true;
            if (sequence == null || !sequence.active)
                sequence = DOTween.Sequence();
            sequence.Append(crystal.transform.DOMove(endTransform, speed).From(startTransform));
        }
    }
    #region MetodDescription
    ///<summary>
    ///Animate crystal destruction
    ///</summary>
    ///<remarks>
    ///!!!WARNING!!! Animations work asynchronously relative to the main thread.
    /// You must wait until the animation finishes for the animation to play correctly.
    /// <list type="table">
    /// <item>
    /// <term>crystal</term>
    /// <description>
    /// GameObject which will be animated
    /// </description>
    /// </item>
    /// <item>
    /// <term>action</term>
    /// <description>
    /// Optional action after animation end. You can connect your action to animation thread here.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <param name="duration">Animation duration. Default = 1f </param>
    /// <param name="action">Optional action after animation end. You can connect your action to animation thread here. Default = null </param>
    #endregion
    public static void AnimateDestroy(GameObject crystal, Action action = null, float duration = 1f)
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
}
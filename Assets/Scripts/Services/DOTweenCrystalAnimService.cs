using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

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
        Color color = GetNonTransparentColor(crystal);
        //crystal.transform.localScale = targetTransform.localScale;

        if (IsAnimated)
            sequence.Join(crystal.transform.DOMove(targetTransform.position, speed))
                    .Join(crystal.GetComponent<Image>().DOColor(color, speed).SetEase(Ease.InExpo));
        
        else
        {
            IsAnimated = true;
            if (sequence == null || !sequence.active)
                sequence = DOTween.Sequence();
            sequence.Append(crystal.transform.DOMove(targetTransform.position, speed))
                    .Join(crystal.GetComponent<Image>().DOColor(color, speed).SetEase(Ease.InExpo));

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
    public static void AnimateDestroy(Crystal crystal, Action action = null, float duration = 1f)
    {
        crystal.dissloveValue = 0f;
        Tween animated = DOTween.To(() => crystal.dissloveValue, // Лямбда-функция, возвращающая начальное значение
                   value => 
                   {
                       crystal.SetMaterialValue(value);
                       crystal.dissloveValue = value;
                   }, // Лямбда-функция, устанавливающая новое значение
                   1, // Конечное значение
                   duration) ; // Длительность анимации


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

    public static void AnimateCreateBomb(GameObject crystal, Transform targetTransform, float speed = 1f)
    {
        Color color = GetNonTransparentColor(crystal);
        //crystal.transform.localScale = targetTransform.localScale;

        if (IsAnimated)
            sequence
                    .Join(crystal.GetComponent<Image>().DOColor(color, speed).SetEase(Ease.InExpo));
        else
        {
            IsAnimated = true;
            if (sequence == null || !sequence.active)
                sequence = DOTween.Sequence();
            sequence.Append(crystal.GetComponent<Image>().DOColor(color, speed).SetEase(Ease.InExpo));

        }
    }

    private static Color GetNonTransparentColor(GameObject crystal)
    {
        Color color = crystal.GetComponent<Image>().color;
        color.a = 1f;
        return color;
    }
}
using System;
using UnityEngine;

public interface ICrystalAnimationService
{
    ///<summary>
    ///Animate crystal movement from current to target position
    ///</summary>
    ///<remarks> 
    /// <list type="table">
    /// <item>
    /// <term>crystal</term>
    /// <description>GameObject which will be animated</description>
    /// </item>
    /// <item>
    /// <term>targetPos</term>
    /// <description>
    /// Target Vector3 position
    /// </description>
    /// </item>
    /// <item>
    /// <term>speed</term>
    /// <description> Speed of animation. Default value - 1f </description>
    ///</item>
    /// </list>
    /// </remarks>
    ///<param name="speed">Animation duration. Default => 1f </param>

    void AnimatePosition(GameObject crystal, Vector3 targetPos, float speed = 1f);

    ///<summary>
    ///Animate crystal movement from current to target position
    ///</summary>
    ///<remarks> 
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
    /// <description> Speed of animation. Default value - 1f </description>
    ///</item>
    /// </list>
    /// </remarks>
    ///<param name="speed">Animation duration. Default => 1f </param>
    void AnimatePosition(GameObject crystal, Transform targetTransform, float speed);

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
    /// <param name="duration">Animation duration. Default => 1f </param>
    /// <param name="action">Optional action after animation end. You can connect your action to animation thread here. Default  = null </param>


    void AnimateDestroy(GameObject crystal, Action action = null, float duration = 5f);

    //void AnimateRotation();


}
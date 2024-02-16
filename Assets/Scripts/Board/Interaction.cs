using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Interaction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector2 _startTouchPosition = Vector2.zero;
    private Vector2 _finishTouchPosition = Vector2.zero;
    public float swipeAngle = 0;

    public event Action<Direction> SwapAction;

    public void OnPointerDown(PointerEventData eventData)
    {
        _startTouchPosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _finishTouchPosition = eventData.position;
        CalculateAngle();
        SwapAction?.Invoke(CalculateDirection());
    }

    void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(_finishTouchPosition.y - _startTouchPosition.y,
            _finishTouchPosition.x - _startTouchPosition.x) * 180 / Mathf.PI;
        Debug.Log($"Angle {swipeAngle}");
    }

    // Oh nein, cringe
    public Direction CalculateDirection()
    {
        if (_finishTouchPosition - _startTouchPosition == Vector2.zero)
            return Direction.None;
        if (swipeAngle > -45f && swipeAngle <= 45f)
            return Direction.Right;
        else if (swipeAngle > 45f && swipeAngle <= 135f)
            return Direction.Top;
        else if ((swipeAngle <= -135f && swipeAngle >= -180) || (swipeAngle > 135f && swipeAngle <= 180))
            return Direction.Left;
        else if (swipeAngle > -135f && swipeAngle <= -45f)
            return Direction.Bottom;
        else
            return Direction.None;
    }


}

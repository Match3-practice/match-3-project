using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Crystal : Movement
{
    public event Action<Direction> Move;

    private void Start()
    {
        SwapAction += OnMove;
    }

    public void OnMove(Direction direction)
    {
        Move?.Invoke(direction);
    }
}

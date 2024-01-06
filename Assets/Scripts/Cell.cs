using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Left,
    Right,
    Top,
    Bottom,
    None
}
public class Cell
{
    public Crystal Crystal
    {
        get => _crystal;
        private set
        {
            _crystal = value;
            if (_crystal == null)
                IsEmpty = false;
            else IsEmpty = true;
        }
    }

    private Crystal _crystal;
    public Direction Gravity;
    public bool IsEmpty { get; private set; } = false;
    private Cell _left_cell;
    private Cell _right_cell;
    private Cell _top_cell;
    private Cell _bottom_cell;


    public Cell(Crystal crystal, Cell left_cell,
        Cell right_cell,Cell top_cell,Cell bottom_cell, Direction gravity)
    {
        Crystal = crystal;
        Crystal.Move += TrySwap;
        _left_cell = left_cell;
        _right_cell = right_cell;
        _top_cell = top_cell;
        _bottom_cell = bottom_cell;
        Gravity = gravity;
    }

    public void TrySwap(Direction direction)
    {
        if (!CanSwap(direction))
        {
            Debug.Log($"Can't swap: direction {direction}");
            return;
        }
        Debug.Log($"Swap: direction {direction}");
        SwapWithNeighbor(GetNeighbor(direction));
    }
    private void SwapWithNeighbor(Cell neighbor)
    {
        Crystal temporary = neighbor.Crystal;
        neighbor.Crystal = Crystal;
        Crystal = temporary;
    }

    private bool CanSwap(Direction direction)
    {
        Cell neighbor = GetNeighbor(direction);
        return neighbor==null? false : !neighbor.IsEmpty;
    }

    private void MoveToEmptySpace(Cell cell)
    {
        Cell neighbor = cell?.GetNeighbor(Gravity);
        if (!neighbor.IsEmpty || neighbor == null)
            return;
        neighbor.Crystal = cell.Crystal;
        cell.Crystal = null;
        MoveToEmptySpace(neighbor);
    }

    public Cell GetNeighbor(Direction direction)
    {
        switch (direction)
        {
            case Direction.Bottom:
                return _bottom_cell;
            case Direction.Top:
                return _top_cell;
            case Direction.Left:
                return _left_cell;
            case Direction.Right:
                return _right_cell;
            default:
                return null;
        }
    }
}

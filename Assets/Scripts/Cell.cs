using System;
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

public struct Neighbors
{
    public Cell _left_cell;
    public Cell _right_cell;
    public Cell _top_cell;
    public Cell _bottom_cell;

    public Cell _left_top_cell;
    public Cell _right_top_cell;
    public Cell _left_bottom_cell;
    public Cell _right_bottom_cell;

}
public class Cell
{
    public Crystal Crystal
    {
        get => _crystal;
        private set
        {
            Crystal oldCrystal = _crystal;
            _crystal = value;
            if (_crystal == null)
            {
                IsEmpty = true;
                oldCrystal.Destroy();
            }
            else
            {
                _crystal.ChangePositionInBoard(this);
                IsEmpty = false;
            }
        }
    }
    public GameObject _prefab;
    private Crystal _crystal;
    public Direction Gravity;
    private Neighbors _neighbors;
    public Vector2 Position;
    public bool IsEmpty { get; private set; } = false;

    public event Action EndSwapping;
    public Cell(Crystal crystal, Direction gravity, GameObject prefab, Board parent)
    {
        _prefab = prefab;
        _crystal = crystal;
        _crystal.SubscribeIntercationAction(TrySwap);
        Gravity = gravity;
        Subscribe(parent);
    }
    public void SetNeighbors(Neighbors neighbors)
    {
        _neighbors = neighbors;
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

        EndSwapping?.Invoke();
    }

    public void UpdatePositionInfo()
    {
        Position = _crystal.Position;
    }

    private bool CanSwap(Direction direction)
    {
        Cell neighbor = GetNeighbor(direction);
        return neighbor == null ? false : !neighbor.IsEmpty;
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

    private void CheckNeighborsMatch(Cell cell, Direction direction)
    {
        Cell neighbor = cell?.GetNeighbor(direction);
        if (neighbor == null || neighbor.Crystal.Type != cell.Crystal.Type)
        {
            cell.Crystal = null;
            return;
        }
        neighbor.Crystal.MustDestroy = true;
        Debug.Log($"Match: {neighbor.Crystal.Type}");
        CheckNeighborsMatch(neighbor, direction);
    }

    public void CheckMatch()
    {
        Cell neighborLeft = GetNeighbor(Direction.Left);
        if (neighborLeft == null || neighborLeft.Crystal == null)
            return;
        Cell neighborRight = GetNeighbor(Direction.Right);
        if (neighborRight == null || neighborRight.Crystal == null)
            return;
        if (neighborLeft.Crystal.Type == Crystal.Type
            && neighborRight.Crystal.Type == Crystal.Type)
        {
            Debug.Log($"Match: {Crystal.Type}");
            Crystal.MustDestroy = true;
            CheckNeighborsMatch(this, Direction.Right);
            CheckNeighborsMatch(this, Direction.Left);
            Crystal = null;
        }
    }
    public Cell GetNeighbor(Direction direction)
    {
        switch (direction)
        {
            case Direction.Bottom:
                return _neighbors._bottom_cell;
            case Direction.Top:
                return _neighbors._top_cell;
            case Direction.Left:
                return _neighbors._left_cell;
            case Direction.Right:
                return _neighbors._right_cell;
            default:
                return null;
        }
    }
    private void Subscribe(Board parent)
    {
        EndSwapping = parent.StartCheckingMatch;
        parent._startCheckingMatch += CheckMatch;
    }

}

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
            _crystal = value;
            if (_crystal == null)
            {
                IsEmpty = true;
            }
            else
            {
                _crystal.ChangePositionInBoard(this);
                IsEmpty = false; 
            }
        }
    }

    private Crystal _crystal;
    public Direction Gravity;
    private Neighbors _neighbors;
    public Vector2 Position;

    public bool IsEmpty { get; private set; } = false;


    public Cell(Crystal crystal, Direction gravity)
    {
        Crystal = crystal;
        Gravity = gravity;
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
        UpdatePositionInfo();
        neighbor.UpdatePositionInfo();

        Crystal temporary = neighbor.Crystal;
        neighbor.Crystal = Crystal;
        Crystal = temporary;
    }

    public void UpdatePositionInfo()
    {
        Position = _crystal.Position;
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
}

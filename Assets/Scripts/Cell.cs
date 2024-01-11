using System;
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

}
public class Cell
{
    private GameObject _prefab;
    private Crystal _crystal;
    public Direction Gravity;
    private Neighbors _neighbors;
    public Transform Position { get => _prefab.transform; }
    public bool IsEmpty { get; private set; } = false;

    public event Action EndSwapping;
    public event Action EndCheckMatching;

    public Crystal Crystal
    {
        get => _crystal;
        private set
        {
            _crystal = value;
            if (_crystal == null)
                IsEmpty = true;
            else
            {
                _crystal.ChangePositionInBoard(this);
                IsEmpty = false;
            }
        }
    }

    #region Initialization
    /// <param name="crystal">Initial crystal</param>
    /// <param name="gravity">Direction of falling crystals</param>
    /// <param name="prefab">Cell prefab</param>
    /// <param name="parent">Parent board where the cell is located</param>
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
    /// <summary>
    /// Subscribe parent and cell events
    /// </summary>
    private void Subscribe(Board parent)
    {
        EndSwapping = parent.EndSwapping;
        EndCheckMatching = parent.CellEndCheckMaching;
        parent._startCheckingMatch += CheckMatch;
    }
    #endregion

    //########################
    //Swapping
    /// <summary>
    /// Checks whether the crystal can be moved and moves it
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <item>
    /// <term>direction</term>
    /// <description>
    /// Swap direction
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <param name="direction">Swap direction</param>
    public void TrySwap(Direction direction)
    {
        if (!CanSwap(direction))
        {
            Debug.Log($"Can't swap: direction {direction}");
            return;
        }
        Debug.Log($"Swap: direction {direction}");
        Cell neighbor = GetNeighbor(direction);
        if (neighbor == null)
            Debug.LogError($"The neighbor doesn't exist. Direction: {direction}");
        SwapWithNeighbor(neighbor);
    }

    /// <summary>
    /// Exchanges crystals with the cell neighbor
    /// </summary>
    /// <param name="neighbor">The adjacent cell 
    /// with which there will be a swap attempt</param>
    private void SwapWithNeighbor(Cell neighbor)
    {
        Crystal temporary = neighbor.Crystal;
        neighbor.Crystal = Crystal;
        Crystal = temporary;
        EndSwapping?.Invoke();
    }

    /// <summary>
    /// Checks whether crystals can be swapped
    /// </summary>
    /// <returns>Returns <see langword="true"/> if a swap can be made</returns>
    private bool CanSwap(Direction direction)
    {
        Cell neighbor = GetNeighbor(direction);
        return neighbor == null ? false : !neighbor.IsEmpty;
    }

    //##################
    //Match

    /// <summary>
    /// The main method for checking match
    /// </summary>
    public void CheckMatch()
    {
        if (Crystal != null)
            CheckMatchByDirection(Direction.Right, GetReverseDirection(Direction.Right));
        if (Crystal != null)
            CheckMatchByDirection(Direction.Top, GetReverseDirection(Direction.Top));
        if (Crystal != null && Crystal.MustDestroy)
        {
            Crystal.MustDestroy = true;
        }
        EndCheckMatching?.Invoke();
    }

    /// <summary>
    /// Ñhecks matches with all neighbors in two directions
    /// </summary>
    /// <param name="direction">Forward direction</param>
    /// <param name="directionReverse">Reverse direction</param>
    private void CheckMatchByDirection(Direction direction, Direction directionReverse)
    {
        if (HasNeighborSameTypeCrystal(direction) && HasNeighborSameTypeCrystal(directionReverse))
        {
            Cell neighborForward = GetNeighbor(direction);
            Cell neighborBackward = GetNeighbor(directionReverse);

            Debug.Log($"Match: {Crystal.Type}");
            CheckNeighborsMatch(neighborForward, direction);
            CheckNeighborsMatch(neighborBackward, directionReverse);
            Crystal.MustDestroy = true;
        }
    }

    ///<summary>
    ///Checks if crystal match with neighbor's crystal
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <item>
    /// <term>direction</term>
    /// <description>
    /// The direction of the neighbor's location relative to the current crystal
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    /// /// <returns>
    /// Returns <see langword="true"/> if the types match and <see langword="false"/>  if the types do not match or the neighbor is empty.
    /// </returns>
    private bool HasNeighborSameTypeCrystal(Direction direction)
    {
        Cell neighbor = GetNeighbor(direction);
        if (neighbor == null || neighbor.Crystal == null)
            return false;
        if (neighbor.Crystal.Type != Crystal.Type)
            return false;
        return true;
    }

    /// <summary>
    /// Checks matches with all neighbors
    /// </summary>
    /// <remarks>
    /// <list type="table">
    /// <item>
    /// <term>cell</term>
    /// <description>
    /// The cell with which we compare neighbors
    /// </description>
    /// </item>
    /// <item>
    /// <term>direction</term>
    /// <description>
    /// The direction of the neighbor's location relative to the current crystal
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    private void CheckNeighborsMatch(Cell cell, Direction direction)
    {
        Cell neighbor = cell?.GetNeighbor(direction);
        //until the neighboring crystal is of a different type
        if (neighbor == null || neighbor.Crystal == null || neighbor.Crystal.Type != cell.Crystal.Type)
        {
            cell.Crystal.MustDestroy = true;
            return;
        }
        Debug.Log($"Match: {neighbor.Crystal.Type}");
        CheckNeighborsMatch(neighbor, direction);
        cell.Crystal.MustDestroy = true;
    }

    //###############
    //Move To Empty Spaces

    public void TryMoveCrystalToEmptySpaces()
    {
        if (Crystal != null)
            MoveToEmptySpace(this);
    }
    private void MoveToEmptySpace(Cell cell)
    {
        Cell neighbor = cell?.GetNeighbor(Gravity);
        if (neighbor == null || !neighbor.IsEmpty)
            return;
        neighbor.Crystal = cell.Crystal;
        cell.Crystal = null;
        MoveToEmptySpace(neighbor);
    }
    
    
    public bool ClearCrystal()
    {
        if (Crystal != null && Crystal.MustDestroy)
        {
            Crystal.Destroy();
            Crystal = null;
            return true;
        }
        return false;
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

    private Direction GetReverseDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Bottom:
                return Direction.Top;
            case Direction.Top:
                return Direction.Bottom;
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;
            default:
                return Direction.Left;
        }
    }
}

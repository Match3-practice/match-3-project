using System;
using UnityEngine;
public struct SwapInfo
{
    public Cell NeighborCell;
    public Direction SwapDirection;
    public SwapInfo(Cell neighborCell, Direction swapDirection)
    {
        this.NeighborCell = neighborCell;
        this.SwapDirection = swapDirection;
    }
    public void Clear()
    {
        NeighborCell = null;
        SwapDirection = Direction.None;
    }
}
public struct MatchInfo
{
    public bool matchVertival;
    public bool matchHorizontal;
    public ushort countHorizontalMatch;
    public ushort countVerticalMatch;

    public MatchInfo(int count = 0)
    {
        matchVertival = false;
        matchHorizontal = false;
        countHorizontalMatch = 0;
        countVerticalMatch = 0;
    }

    public void Match(Direction direction)
    {
        if (direction == Direction.Left || direction == Direction.Right)
            HorizontalMatch();
        else if (direction == Direction.Top || direction == Direction.Bottom)
            VerticalMatch();
    }
    private void VerticalMatch()
    {
        countVerticalMatch++;
        if (countVerticalMatch >= 3)
            matchVertival = true;
        //if (countVerticalMatch == 4)
        //    Debug.Log("Must create Vertical bomb");
    }

    private void HorizontalMatch()
    {
        matchHorizontal = true;
        countHorizontalMatch++;
        if (countHorizontalMatch >= 3)
            matchHorizontal = true;
        //if (countHorizontalMatch == 4)
        //    Debug.Log("Must create Horizontal bomb");
    }

}
public class Cell : MonoBehaviour
{
    [HideInInspector] public Direction Gravity;

    private Crystal _crystal;
    private Neighbors _neighbors;
    public Transform Position { get => gameObject.transform; }
    public bool IsEmpty { get; private set; } = false;

    private SwapInfo _lastSwapInfo;

    public event Action<Cell> StartSwapping;
    public event Action EndSwapping;
    public event Action EndCheckMatching;
    public event Action FoundCrystalToDestroy;
    public event Action<Cell, BombType> MustCreateBomb;

    private MatchInfo _matchInfo;
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

    public void InitializeCrystal(Crystal crystal)
    {
        Crystal = crystal;
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

    public bool ClearCrystal()
    {
        if (Crystal != null && Crystal.MustDestroy)
        {
            Crystal.DestroyAction();

            Crystal = null;
            return true;
        }
        return false;
    }

    public void InitialzeCell(Crystal crystal, Direction gravity, Board parent)
    {
        _crystal = crystal;
        _crystal.SubscribeIntercationAction(TrySwap);
        Gravity = gravity;
        Subscribe(parent);
    }

    public void TryMoveCrystalToEmptySpaces()
    {
        if (Crystal != null)
        {
            MoveToEmptySpace(this);
        }
    }

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
        StartSwapping?.Invoke(this);
        SwapWithNeighbor(neighbor);
        SaveSwapInfo(neighbor, direction);
        neighbor.SaveSwapInfo(this, direction);
        CheckMatchAfterSwap();
        neighbor.CheckMatchAfterSwap();
        EndSwapping?.Invoke();
    }
    private void SaveSwapInfo(Cell cellToSwap, Direction swapDirection)
    {
        _lastSwapInfo = new SwapInfo(cellToSwap, swapDirection);
    }
    public void SetNeighbors(Neighbors neighbors)
    {
        _neighbors = neighbors;
    }

    //check match after end all swap
    private void CheckMatch()
    {
        _matchInfo = new MatchInfo();
        CheckAllCombinations();
       if (!Result())
            EndCheckMatch();
    }
    //check match immediately after swap
    private void CheckMatchAfterSwap()
    {
        _matchInfo = new MatchInfo();
        CheckAllCombinations();
        Result();
    }

    private bool Result()
    {
        if (_matchInfo.countHorizontalMatch > 3)
        {
            MustCreateBomb?.Invoke(this, BombType.Horizontal);
            Debug.Log("Must create Horizontal bomb");
            return true;
        }
        if (_matchInfo.countVerticalMatch > 3)
        {
            MustCreateBomb?.Invoke(this, BombType.Vertical);
            Debug.Log("Must create Vertical bomb");
            return true;
        }
        return false;

    }
    private void CheckAllCombinations()
    {
        if (Crystal != null)
        {
            //check the match on both sides
            bool hasMatch3 = CheckMatchBy2Direction(Direction.Right, GetReverseDirection(Direction.Right));
            if (!hasMatch3)
            {
                //check the match on one side only
                CheckMatchBy1Direction(Direction.Right);
                //check the match on one side only
                CheckMatchBy1Direction(Direction.Left);
            }
        }
        if (Crystal != null)
        {
            //check the match on both sides
            bool hasMatch3 = CheckMatchBy2Direction(Direction.Top, GetReverseDirection(Direction.Top));
            if (!hasMatch3)
            {
                CheckMatchBy1Direction(Direction.Top);
                CheckMatchBy1Direction(Direction.Bottom);
            }
        }
    }


    private void EndCheckMatch()
    {
        EndCheckMatching?.Invoke();
    }
    private bool CheckMatchBy2Direction(Direction direction, Direction directionReverse)
    {
        if (HasNeighborSameTypeCrystal(direction) && HasNeighborSameTypeCrystal(directionReverse))
        {
            Cell neighborForward = GetNeighbor(direction);
            Cell neighborBackward = GetNeighbor(directionReverse);

            if (neighborForward.Crystal.MustDestroy || neighborBackward.Crystal.MustDestroy)
                return false;
            CheckNeighborsMatch(neighborForward, direction);
            CheckNeighborsMatch(neighborBackward, directionReverse);
            MarkCrystalToDestroy(Crystal);
            FoundCrystalToDestroy?.Invoke();
            _matchInfo.Match(direction);
            return true;
        }
        return false;
    }
    private void CheckMatchBy1Direction(Direction direction)
    {
        Cell neighbor1 = GetNeighbor(direction);
        Cell neighbor2 = neighbor1?.GetNeighbor(direction);
        if (neighbor1 == null || neighbor2 == null || neighbor1.Crystal.MustDestroy || neighbor2.Crystal.MustDestroy)
            return;
        if (Crystal.Type == neighbor1.Crystal.Type && Crystal.Type == neighbor2.Crystal.Type)
        {
            CheckNeighborsMatch(neighbor1, direction);
            MarkCrystalToDestroy(Crystal);
            FoundCrystalToDestroy?.Invoke();
            _matchInfo.Match(direction);
        }
    }


    private void CheckNeighborsMatch(Cell cell, Direction direction)
    {
        Cell neighbor = cell?.GetNeighbor(direction);
        //until the neighboring crystal is of a different type
        if (neighbor == null || neighbor.Crystal == null || neighbor.Crystal.Type != cell.Crystal.Type)
        {
            MarkCrystalToDestroy(cell.Crystal);
            _matchInfo.Match(direction);
            return;
        }
        CheckNeighborsMatch(neighbor, direction);
        MarkCrystalToDestroy(cell.Crystal);
        _matchInfo.Match(direction);
    }
    public void Restore()
    {
        if (CheckIfSwapBackNeed())
        {
            SwapBack();
        }
    }
    private bool CheckIfSwapBackNeed()
    {
        Cell lastSwapNeighbor = _lastSwapInfo.NeighborCell;
        if (lastSwapNeighbor == null)
            return false;
        return true;
    }
    private void MoveToEmptySpace(Cell cell)
    {
        Cell neighbor = cell?.GetNeighbor(Gravity);
        if (neighbor == null || !neighbor.IsEmpty)
        {
            return;
        }
        //if (cell.Crystal != null)
        //    Debug.Log(neighbor.gameObject.name);
        //Debug.Log(cell.gameObject.name);


        DebugRay(cell);
        neighbor.Crystal = cell.Crystal;
        cell.Crystal = null;
        MoveToEmptySpace(neighbor);
    }

    private void DebugRay(Cell cell)
    {
        switch (cell.Crystal.Type)
        {
            case Types.Red:
                Debug.DrawRay(cell.Crystal.transform.position, Vector3.down * 100f, Color.red, 30f);

                break;
            case Types.Blue:
                Debug.DrawRay(cell.Crystal.transform.position, Vector3.down * 100f, Color.blue, 30f);

                break;
            case Types.Green:
                Debug.DrawRay(cell.Crystal.transform.position, Vector3.down * 100f, Color.green, 30f);

                break;
            default:
                break;
        }
    }
    private void MarkCrystalToDestroy(Crystal crystal)
    {
        crystal.MustDestroy = true;
    }
    private void Subscribe(Board parent)
    {
        StartSwapping = parent.StartSwapping;
        EndSwapping = parent.EndSwapping;
        EndCheckMatching = parent.CellEndCheckMaching;
        parent._startCheckingMatch += CheckMatch;
        FoundCrystalToDestroy = () => parent.MustUpdateBoard = true;
        MustCreateBomb += parent.NeedCreateBomb;
    }

    private void SwapWithNeighbor(Cell neighbor)
    {
        Crystal temporary = neighbor.Crystal;
        neighbor.Crystal = Crystal;
        Crystal = temporary;
    }
    private void SwapBack()
    {
        Cell neighbor = _lastSwapInfo.NeighborCell;
        if (neighbor == null)
        {
            Debug.LogError($"The neighbor doesn't exist. Direction: {_lastSwapInfo.SwapDirection}");
            return;
        }
        _lastSwapInfo.Clear();
        SwapWithNeighbor(neighbor);
    }
    private bool CanSwap(Direction direction)
    {
        Cell neighbor = GetNeighbor(direction);
        return neighbor == null ? false : !neighbor.IsEmpty;
    }

    private bool HasNeighborSameTypeCrystal(Direction direction)
    {
        Cell neighbor = GetNeighbor(direction);
        if (neighbor == null || neighbor.Crystal == null)
            return false;
        if (neighbor.Crystal.Type != Crystal.Type)
            return false;
        return true;
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
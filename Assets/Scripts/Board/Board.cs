using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    [SerializeField] private CrystalData[] _setOfCrystals;

    [SerializeField] private GameObject _cellPrefab;

    [SerializeField] [Min(3)] private int _width = 5;
    [SerializeField] [Min(3)] private int _height = 5;
    [SerializeField] private float _spacing = 10f;

    [SerializeField] private bool _isBoardCreated = true;

    public event Action _startCheckingMatch;

    public Direction Gravity = Direction.Bottom;

    public Cell[] Cells { get; private set; }
    public int Width { get => _width; }
    public int Height { get => _height; }

    private int _cellCount;
    private bool _isNeedClearCrystals = false;

    private Vector3[] _spawnPoints;

    private void Start()
    {
        if (_isBoardCreated)
            InitializeBoard();
        else
            InitializeBoardFromZero();
    }

    private void InitializeBoard()
    {
        _spawnPoints = new Vector3[_width];
        Cells = gameObject.GetComponentsInChildren<Cell>();
        for (int i = 0; i < _height * _width; i++)
        {
            Crystal crystal = GenerateCrystalInCell(Cells[i].gameObject, i);

            Cells[i].InitialzeCell(crystal, Gravity, this);

            Cells[i].SetNeighbors(FillNeighbors(i));
        }
    }
    public void InitializeBoardFromZero()
    {
        _spawnPoints = new Vector3[_width];
        Cells = new Cell[_width * _height];
        for (int i = 0; i < _height * _width; i++)
        {
            GameObject cellObject = Instantiate(_cellPrefab, transform);
            Cells[i] = cellObject.GetComponent<Cell>();
        }
        for (int i = 0; i < _height * _width; i++)
        {
            Crystal crystal = GenerateCrystalInCell(Cells[i].gameObject, i);

            Cells[i].InitialzeCell(crystal, Gravity, this);

            Cells[i].SetNeighbors(FillNeighbors(i));
        }
    }
    private void InitializeSpawnPosition()
    {
        Cell[] row = GetCellsToSpawnIn();
        for (int i = 0; i < row.Length; i++)
        {
            Vector3 cellPosition = row[i].transform.position;
            switch (Gravity)
            {
                case Direction.Bottom:
                    _spawnPoints[i] = new Vector3(cellPosition.x, cellPosition.y + _spacing, cellPosition.z);
                    break;
                case Direction.Top:
                    _spawnPoints[i] = new Vector3(cellPosition.x, cellPosition.y - _spacing, cellPosition.z);
                    break;
                case Direction.Left:
                    _spawnPoints[i] = new Vector3(cellPosition.x + _spacing, cellPosition.y, cellPosition.z);
                    break;
                case Direction.Right:
                    _spawnPoints[i] = new Vector3(cellPosition.x - _spacing, cellPosition.y, cellPosition.z);
                    break;
            }
        }
    }

    private Cell[] GetCellsToSpawnIn()
    {
        Cell[] firstRow;
        switch (Gravity)
        {
            case Direction.Bottom:
                {
                    firstRow = new Cell[_width];
                    for (int i = 0, k = 0; i < _width; i++, k++)
                    {
                        firstRow[k] = Cells[i];
                    }
                    return firstRow;
                }
            case Direction.Top:
                {
                    firstRow = new Cell[_width];
                    for (int i = (_width - 1) * _height, k = 0; i < _width * _height; i++, k++)
                    {
                        firstRow[k] = Cells[i];
                    }
                    return firstRow;
                }
            case Direction.Left:
                firstRow = new Cell[_height];
                for (int i = _width - 1, k = 0; i < _width * _height; i += _width, k++)
                {
                    firstRow[k] = Cells[i];
                }
                return firstRow;
            case Direction.Right:
                firstRow = new Cell[_height];
                for (int i = 0, k = 0; i < _width * _height; i += _width, k++)
                {
                    firstRow[k] = Cells[i];
                }
                return firstRow;
            default:
                return null;
        }
    }
    //Works after swap is complete
    public void EndSwapping()
    {
        DOTweenCrystalAnimService.EndAnimations();
        StartCheckingMatch();
    }

    public void StartCheckingMatch()
    {
        _cellCount = _height * _width;
        _startCheckingMatch?.Invoke();
    }

    public void CellEndCheckMaching()
    {
        _cellCount--;
        if (_cellCount == 0)
        {
            ClearMustDestroyedCrystals();
            DOTweenCrystalAnimService.EndAnimations();
            CheckEmptySpaces();
            DOTweenCrystalAnimService.EndAnimations();
            SpawnCrystalsAfterStep();
            DOTweenCrystalAnimService.EndAnimations();
            if (_isNeedClearCrystals)
                StartCheckingMatch();
        }
    }
    //spawn crystals in empty cells of
    //the first row or column
    public void SpawnCrystalsAfterStep()
    {
        InitializeSpawnPosition();
        bool hasEmptyCells = true;
        while (hasEmptyCells)
        {
            hasEmptyCells = false;
            Cell[] cellsToSpawn = GetCellsToSpawnIn();
            for (int i = 0; i < cellsToSpawn.Length; i++)
            {
                if (cellsToSpawn[i].IsEmpty)
                {
                    hasEmptyCells = true;
                    Crystal newCrystal = GenerateCrystalInPoint(_spawnPoints[i]);
                    cellsToSpawn[i].InitializeCrystal(newCrystal);
                }
            }
            DOTweenCrystalAnimService.EndAnimations();
            CheckEmptySpaces();
        }
    }

    public void ClearMustDestroyedCrystals()
    {
        _isNeedClearCrystals = false;
        for (int i = 0; i < _height * _width; i++)
        {
            bool result = Cells[i].ClearCrystal();
            if (result)
                _isNeedClearCrystals = true;
        }
    }

    public void CheckEmptySpaces()
    {
        for (int i = _height * _width - 1; i >= 0; i--)
        {
            Cells[i].TryMoveCrystalToEmptySpaces();
        }
    }

    private Neighbors FillNeighbors(int index)
    {
        Neighbors neighbors = new Neighbors();

        if (Cells == null)
            return neighbors;

        if (index % _width != 0)
            neighbors._left_cell = Cells[index - 1];
        if (index % _width != _width - 1)
            neighbors._right_cell = Cells[index + 1];

        if (index >= _width)
            neighbors._top_cell = Cells[index - _width];
        if (index < Cells.Length - _width)
            neighbors._bottom_cell = Cells[index + _width];
        return neighbors;
    }

    private Crystal GenerateCrystalInCell(GameObject cell, int currentIndex = -1)
    {
        if (_setOfCrystals.Length == 0)
        {
            Debug.LogError("The set of crystals is not filled. Check the field \"Set Of Crystals\"");
            return null;
        }

        CrystalData crystalData = ChooseCrystalToSpawn(currentIndex);
        GameObject crystalPrefab = Instantiate(crystalData.Prefab, cell.transform);
        Crystal crystal = crystalPrefab.GetComponent<Crystal>();
        crystal.Type = crystalData.Type;
        return crystal;
    }

    private Crystal GenerateCrystalInPoint(Vector3 point)
    {
        if (_setOfCrystals.Length == 0)
        {
            Debug.LogError("The set of crystals is not filled. Check the field \"Set Of Crystals\"");
            return null;
        }
        CrystalData crystalData = _setOfCrystals[UnityEngine.Random.Range(0, _setOfCrystals.Length)];
        GameObject crystalPrefab = Instantiate(crystalData.Prefab, point, new Quaternion());
        MakeTransparent(crystalPrefab);
        Crystal crystal = crystalPrefab.GetComponent<Crystal>();
        crystal.Type = crystalData.Type;
        return crystal;
    }

    private CrystalData ChooseCrystalToSpawn(int cellIndex)
    {
        if (cellIndex < 0)
            return null;
        CrystalData crystalData = null;
        ArrayList crystals = new ArrayList();
        crystals.AddRange(_setOfCrystals);

        //get the element to the left of
        //the current crystal if there is one
        int leftIndex = cellIndex - 1;
        Cell leftCell = cellIndex % _width == 0 || leftIndex < 0 ? null : Cells[leftIndex];
        //get the element on top from the current
        //crystal, if there is one
        int topIndex = cellIndex - _width;
        Cell topCell = topIndex < 0 ? null : Cells[topIndex];

        if (leftCell != null || topCell != null)
        {
            //get the element to the left one
            //position from the current one
            int lefLeftIndex = cellIndex - 2;
            Cell leftLeftCell = cellIndex % _width == 1 || lefLeftIndex < 0 ? null : Cells[lefLeftIndex];
            //get the element one position
            //above the current one
            int topTopIndex = cellIndex - _width * 2;
            Cell topTopCell = topTopIndex < 0 ? null : Cells[topTopIndex];

            //checking if two crystals to the left of
            //the current one are the same
            bool isHorizontalMatch = leftLeftCell?.Crystal.Type == leftCell?.Crystal.Type;
            //checking if two crystals above the
            //current one are the same
            bool isVerticalMatch = topTopCell?.Crystal.Type == topCell?.Crystal.Type;

            if (isHorizontalMatch || isVerticalMatch)
            {
                //remove from the list a possible crystal for spawning
                foreach (CrystalData crystal in _setOfCrystals)
                {
                    if (crystal.Type == leftCell?.Crystal.Type)
                        crystals.Remove(crystal);
                    if (crystal.Type == topCell?.Crystal.Type)
                        crystals.Remove(crystal);
                }
            }
        }

        if (crystals.Count == 0)
        {
            Debug.LogError("There are no crystals left for spawning! Crystal selected randomly. Be careful, there may be repetitions!");
            crystalData = _setOfCrystals[UnityEngine.Random.Range(0, _setOfCrystals.Length)];
        }
        else
        {
            //select a crystal randomly from the remaining ones
            crystalData = (CrystalData)crystals[UnityEngine.Random.Range(0, crystals.Count)];
        }
        return crystalData;
    }
    private static void MakeTransparent(GameObject crystalPrefab)
    {
        Color color = crystalPrefab.GetComponent<Image>().color;
        color.a = 0f;
        crystalPrefab.GetComponent<Image>().color = color;
    }
}
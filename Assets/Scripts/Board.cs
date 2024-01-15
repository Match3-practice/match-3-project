using System;

using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    private Cell[,] Cells;
    [SerializeField]
    private int _width = 5;
    [SerializeField]
    private int _height = 5;
    public Direction Gravity;

    [SerializeField]
    private CrystalData[] _setOfCrystals;
    [SerializeField]
    private GameObject _cellPrefab;

    public event Action _startCheckingMatch;

    private int _cellCount;
    private bool _isNeedClearCrystals = false;
    void Start()
    {

        InitializeBoard();
    }

    void InitializeBoard()
    {
        //initialize cell array
        Cells = new Cell[_width, _height];
        for (int i = 0; i < _height; i++)
        {
            for (int j = 0; j < _width; j++)
            {
                //instantiate cell prefab
                GameObject cellObject = Instantiate(_cellPrefab, transform);
                //Instantiate crystal
                Crystal crystal = GenerateCrystalInCell(cellObject);
                //create cell instance
                Cells[j, i] = new Cell(crystal, Gravity, cellObject, this);
            }
        }

        //set neighbors for each cell
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                Cells[i, j].SetNeighbors(FillNeighbors(i, j));
            }
        }
    }

    //Works after swap is complete
    public void EndSwapping()
    {
        StartCheckingMatch();
    }
    public void StartCheckingMatch()
    {
        Debug.Log("Start Cheking");
        _cellCount = _height * _width;
        _startCheckingMatch?.Invoke();
    }

    public void CellEndCheckMaching()
    {
        _cellCount--;
        if (_cellCount == 0)
        {
            ClearMustDestroyedCrystals();
            CheckEmptySpaces();
            if (_isNeedClearCrystals)
                StartCheckingMatch();
        }
    }
    public void ClearMustDestroyedCrystals()
    {
        _isNeedClearCrystals = false;
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                bool result = Cells[i, j].ClearCrystal();
                if (result)
                    _isNeedClearCrystals = true;
            }
        }
    }
    public void CheckEmptySpaces()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = _height - 1; j >= 0; j--)
            {

                Cells[i, j].TryMoveCrystalToEmptySpaces();
            }
        }
    }
    private Neighbors FillNeighbors(int indexI, int indexJ)
    {
        Neighbors neighbors = new Neighbors();
        if (Cells == null)
            return neighbors;

        if (indexI > 0)
            neighbors._left_cell = Cells[indexI - 1, indexJ];
        if (indexI + 1 < _width)
            neighbors._right_cell = Cells[indexI + 1, indexJ];
        if (indexJ > 0)
            neighbors._top_cell = Cells[indexI, indexJ - 1];
        if (indexJ + 1 < _height)
            neighbors._bottom_cell = Cells[indexI, indexJ + 1];
        return neighbors;
    }

    private Crystal GenerateCrystalInCell(GameObject cell)
    {
        try
        {
            CrystalData crystalData = _setOfCrystals[UnityEngine.Random.Range(0, _setOfCrystals.Length)];
            GameObject crystalPrefab = Instantiate(crystalData.Prefab, cell.transform);
            Crystal crystal = crystalPrefab.GetComponent<Crystal>();
            crystal.Type = crystalData.Type;
            return crystal;
        }
        catch (IndexOutOfRangeException)
        {
            Debug.LogError("The set of crystals is not filled. Check the field \"Set Of Crystals\"");
        }
        return null;
    }
}

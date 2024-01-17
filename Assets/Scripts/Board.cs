using System;

using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private CrystalData[] _setOfCrystals;

    [SerializeField] private GameObject _cellPrefab;

    [SerializeField] private int _width = 5;
    [SerializeField] private int _height = 5;

    public event Action _startCheckingMatch;

    public Direction Gravity;

    private MonoCell[] Cells;

    private int _cellCount;
    private bool _isNeedClearCrystals = false;

    private void Start()
    {
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        Cells = gameObject.GetComponentsInChildren<MonoCell>();

        for (int i = 0; i < _height * _width; i++)
        {
            Crystal crystal = GenerateCrystalInCell(Cells[i].gameObject);

            Cells[i].InitialzeCell(crystal, Gravity, this);

            Cells[i].SetNeighbors(FillNeighbors(i));
        }
    }

    //Works after swap is complete
    public void EndSwapping()
    {
        #region Debug

        Debug.Log($" CheckMatch count: {MonoCell._counter}");

        #endregion


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
        for (int i = 0; i < _height * _width; i++)
        {

            bool result = Cells[i].ClearCrystal();
            if (result)
                _isNeedClearCrystals = true;
        }
    }
    public void CheckEmptySpaces()
    {
        for (int i = 0; i < _height * _width; i++)
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
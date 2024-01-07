using System;
using System.Collections;
using System.Collections.Generic;

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

    void Start()
    {
        InitializeBoard();
    }

    void InitializeBoard()
    {
        Cells = new Cell[_width, _height];
        for (int i = 0; i < _height; i++)
        {
            for (int j = 0; j < _width; j++)
            {
                Crystal crystal = GenerateCrystal();
                Cells[j, i] = new Cell(crystal, Gravity);
            }
        }

        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                Cells[i, j].SetNeighbors(FillNeighbors(i, j));
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
        if(indexI + 1 < _width)
            neighbors._right_cell = Cells[indexI + 1, indexJ];
        if (indexJ > 0)
            neighbors._top_cell = Cells[indexI, indexJ-1];
        if (indexJ + 1 < _height)
            neighbors._bottom_cell = Cells[indexI, indexJ+1];
        return neighbors;
    }

    private Crystal GenerateCrystal()
    {
        try
        {
            CrystalData crystalData = _setOfCrystals[UnityEngine.Random.Range(0, _setOfCrystals.Length)];
            GameObject crystalPrefab = Instantiate(crystalData.Prefab, transform);
            return crystalPrefab.GetComponent<Crystal>();
        }
        catch (IndexOutOfRangeException)
        {
            Debug.LogError("The set of crystals is not filled. Check the field \"Set Of Crystals\"");
        }
        return null;
    }



}

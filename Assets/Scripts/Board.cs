using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

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
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                Crystal crystal = GenerateCrystal();
                //Don't forget to change this!!!
                Cells[i, j] = new Cell(crystal, null, null, null, null, Gravity);
            }
        }
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

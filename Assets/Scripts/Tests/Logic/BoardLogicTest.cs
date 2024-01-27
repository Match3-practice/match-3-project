
using NUnit.Framework;
using System.Text;
using UnityEngine;

public class BoardLogicTest
{
    StringBuilder log = new StringBuilder();
    private Board _board;
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        log = new StringBuilder();
        GameObject boardObject =
        MonoBehaviour.Instantiate(Resources.Load<GameObject>("For Tests/Board"));
        _board = boardObject.GetComponent<Board>();
    }
    [Test]
    public void CheckSpawnCrystalsWithoutMetch()
    {
        _board.InitializeBoardFromZero();
        int width = _board.Width;
        Cell[] cells = _board.Cells;
        bool isMatchExist = false;
        
        for (int i = 0; i < cells.Length; i++)
        {
            if (i % width > 1)
            {
                Crystal crystal1 = cells[i - 2]?.Crystal;
                Crystal crystal2 = cells[i - 1]?.Crystal;
                Crystal crystal3 = cells[i]?.Crystal;
                if (crystal1.Type == crystal2.Type && crystal2.Type == crystal3.Type)
                {
                    AddToLog("Match:");
                    AddToLog($"Crystal in cell {i - 2} - type {crystal1.Type}");
                    AddToLog($"Crystal in cell {i - 1} - type {crystal2.Type}");
                    AddToLog($"Crystal in cell {i} - type {crystal3.Type}");
                    isMatchExist = true;
                }
            }
            if (i - width * 2 >= 0)
            {
                Crystal crystal1 = cells[i - width * 2]?.Crystal;
                Crystal crystal2 = cells[i - width]?.Crystal;
                Crystal crystal3 = cells[i]?.Crystal;
                if (crystal1.Type == crystal2.Type && crystal2.Type == crystal3.Type)
                {
                    AddToLog("Match:");
                    AddToLog($"Crystal in cell {i - width * 2} - type {crystal1.Type}");
                    AddToLog($"Crystal in cell {i - width} - type {crystal2.Type}");
                    AddToLog($"Crystal in cell {i} - type {crystal3.Type}");
                    isMatchExist = true;
                }
            }
        }
        Assert.False(isMatchExist, log.ToString());
        Assert.Pass("No match found.");
    }
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        Object.DestroyImmediate(_board);
        ClearLog();
    }
    private void AddToLog(string text)
    {
        log.Append(text+"\n");
    }
    private void ClearLog()
    {
        log.Clear();
    }

}

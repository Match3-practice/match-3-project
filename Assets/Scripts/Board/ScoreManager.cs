using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int targetScore = 9;

    [SerializeField] private Text scoreLabel;

    public event Action OnScoreChange;
    public event Action OnWin;
    public static event Action<Types> OnCrystalDestroy;

    private const int DEF_CRYSTAL_SCORE = 1;

    private int score;

    public int Score
    {
        get => score;
        set
        {
            score = value;
            OnScoreChange?.Invoke();
        }
    }


    private void Awake()
    {
        OnScoreChange += WinChecker;
        OnScoreChange += UpdateScoreLabel;
        OnCrystalDestroy += CrysralDestroyHandler;
    }

    public static void InvokeOnCrystalDestroy(Types type)
    {
        OnCrystalDestroy?.Invoke(type);
    }

    private void UpdateScoreLabel()
    {
        scoreLabel.text = Convert.ToString(Score);
    }

    private void CrysralDestroyHandler(Types types)
    {
        //It`s temporary solution
        Score += DEF_CRYSTAL_SCORE;
    }

    private void WinChecker()
    {
        if (Score >= targetScore)
        {
            OnWin?.Invoke();
        }
    }
}
using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int targetScore = 9;

    public event Action OnScoreChange;
    public event Action OnWin;

    private const string SCORE_TAG = "Score";
    private const int DEF_CRYSTAL_SCORE = 1;

    public int Score
    {
        get => GetScore();
        set
        {
            AddScore(value);
            OnScoreChange?.Invoke();
        }
    }

    private void Awake()
    {
        OnScoreChange += WinChecker;
        Cell.OnCrystalDestroy += CrysralDestroyHandler;
    }

    private void CrysralDestroyHandler(Types types)
    {
        //It`s temporary solution
        AddScore(DEF_CRYSTAL_SCORE);
    }

    private void WinChecker()
    {
        if(Score >= targetScore)
        {
            OnWin?.Invoke();
        }
    }

    private void AddScore(int score)
    {
        PlayerPrefs.SetInt(SCORE_TAG, PlayerPrefs.GetInt(SCORE_TAG) + score);
    }

    //private void ClearScore()
    //{
    //    uint zero = uint.MinValue;
    //    Score = (int)zero;
    //}
    private int GetScore()
    {
        return PlayerPrefs.GetInt(SCORE_TAG);
    }
}
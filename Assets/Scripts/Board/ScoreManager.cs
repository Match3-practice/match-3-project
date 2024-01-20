using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int targetScore = 9;

    public event Action OnScoreChange;
    public event Action OnWin;

    private void Awake()
    {
        OnScoreChange += WinChecker;
    }
    public int TargetScore
    {
        get => GetScore();
        set
        {
            AddScore(value);
            OnScoreChange?.Invoke();
        }
    }

    private const string SCORE_TAG = "Score";


    private void WinChecker()
    {
        if(TargetScore >= targetScore)
        {
            OnWin?.Invoke();
        }
    }


    private int GetScore()
    {
        return PlayerPrefs.GetInt(SCORE_TAG);
    }

    private void AddScore(int score)
    {
        PlayerPrefs.SetInt(SCORE_TAG, PlayerPrefs.GetInt(SCORE_TAG) + score);
    }

    public void ClearScore()
    {
        uint zero = uint.MinValue;
        TargetScore = (int)zero;
    }
}
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private const string SCORE_TAG = "Score";

    public int GetScore()
    {
        return Score;
    }

    public void AddScore(int score)
    {


        int x = PlayerPrefs.GetInt(SCORE_TAG) == 0 ? 1 : 2;
        PlayerPrefs.SetInt(SCORE_TAG,PlayerPrefs.GetInt(SCORE_TAG) + score);
        //PlayerPrefs.SetInt(SCORE_TAG, 

    }



}

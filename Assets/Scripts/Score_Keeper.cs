using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Score_Keeper
{
    private static int score = 0;

    public static int Score { get => score; }

    private const string saveFileName = "HighScoreSave";

    [Serializable]
    public class HighScoreList
    {
        public int[] highScores = new int[10];

        public int[] HighScores { get => highScores; }
    }

    private static HighScoreList scoreList = new();

    public static HighScoreList ScoreList { get => scoreList; }

    public static void ResetScore()
    {
        score = 0;
    }

    public static void AddScore(int addedScore)
    {
        score += addedScore;
    }

    public static void LoadHighScores()
    {
        if (!PlayerPrefs.HasKey(saveFileName))
        {
            SaveHighScores();
        }

        string json = PlayerPrefs.GetString(saveFileName, "0");

        if(json != "0")
        {
            scoreList = JsonUtility.FromJson<HighScoreList>(json);
        }
    }

    public static void SaveHighScores()
    {
        int scoreToMoveDown = -1;

        for(int i = 0; i < scoreList.HighScores.Length; i++)
        {
            if(score > scoreList.HighScores[i])
            {
                scoreToMoveDown = scoreList.HighScores[i];

                scoreList.HighScores[i] = score;

                score = 0;
            }
            else if(scoreToMoveDown > scoreList.HighScores[i])
            {
                (scoreToMoveDown, scoreList.HighScores[i]) = (scoreList.HighScores[i], scoreToMoveDown);
            }

            Debug.LogWarning(scoreList.HighScores[i]);
        }

        string json = JsonUtility.ToJson(scoreList);

        PlayerPrefs.SetString(saveFileName, json);
    }

    public static void DeleteAllScores()
    {
        PlayerPrefs.DeleteKey(saveFileName);
    }
}

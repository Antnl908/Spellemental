using System;
using UnityEngine;

public static class Score_Keeper
{
    //Made by Daniel.

    private static int score = 0;

    public static int Score { get => score; }

    private const string saveFileName = "HighScoreSave";

    /// <summary>
    /// A class used to save all scores to a json string.
    /// </summary>
    [Serializable]
    public class HighScoreList
    {
        //Does not serialize if it is not public.
        public int[] highScores = new int[10];

        public int[] HighScores { get => highScores; }
    }

    private static HighScoreList scoreList = new();

    public static HighScoreList ScoreList { get => scoreList; }

    /// <summary>
    /// Puts the score back to zero.
    /// </summary>
    public static void ResetScore()
    {
        score = 0;
    }

    /// <summary>
    /// Adds points to the score.
    /// </summary>
    /// <param name="addedScore">The amount of points added to the score</param>
    public static void AddScore(int addedScore)
    {
        score += addedScore;
    }

    /// <summary>
    /// Loads all of the high scores from a file using PlayerPrefs.
    /// </summary>
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

    /// <summary>
    /// Saves the 10 highest scores to a file using json and PlayerPrefs.
    /// </summary>
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

    /// <summary>
    /// Deletes all of the saved scores.
    /// </summary>
    public static void DeleteAllScores()
    {
        PlayerPrefs.DeleteKey(saveFileName);
    }
}

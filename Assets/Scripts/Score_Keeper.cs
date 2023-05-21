using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Score_Keeper
{
    private static int score = 0;

    public static int Score { get => score; }

    public static void ResetScore()
    {
        score = 0;
    }

    public static void AddScore(int addedScore)
    {
        score += addedScore;
    }
}

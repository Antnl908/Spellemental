using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score_Saver : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI deathSceneScoreText;

    // Start is called before the first frame update
    void Start()
    {
        deathSceneScoreText.text = Score_Keeper.Score.ToString();

        Score_Keeper.SaveHighScores();
    }
}

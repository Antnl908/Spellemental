using UnityEngine;
using TMPro;

public class Display_Score : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    // Update is called once per frame
    void Update()
    {
        scoreText.text = Score_Keeper.Score + " points";
    }
}

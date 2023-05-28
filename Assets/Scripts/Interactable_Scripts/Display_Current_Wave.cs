using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Display_Current_Wave : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI waveText;

    // Update is called once per frame
    void Update()
    {
        waveText.text = Spawner_With_Increasing_Difficulty.CurrentWave.ToString();
    }
}

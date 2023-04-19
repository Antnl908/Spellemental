using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMain : MonoBehaviour
{
    [SerializeField]
    private string main = "Start_Scene";

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(main);
    }
}

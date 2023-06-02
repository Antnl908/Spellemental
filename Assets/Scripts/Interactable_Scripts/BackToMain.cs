using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMain : MonoBehaviour
{
    [SerializeField]
    private string main = "Start_Scene";

    /// <summary>
    /// Loads the main menu scene.
    /// </summary>
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(main);
    }
}

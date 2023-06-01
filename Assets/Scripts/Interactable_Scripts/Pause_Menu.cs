using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Pause_Menu : MonoBehaviour
{
    private Player_Controls controls;

    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private string startMenu = "Start_Scene";

    [SerializeField]
    private Player_Look look;

    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        controls = new();

        controls.Player1.Pause.performed += SwapPauseState;

        controls.Player1.Enable();

        UnPause();
    }

    private void OnDisable()
    {
        controls.Player1.Pause.performed -= SwapPauseState;
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;

        Score_Keeper.SaveHighScores();

        SceneManager.LoadScene(startMenu);
    }

    private void SwapPauseState(InputAction.CallbackContext context)
    {
        if (isPaused)
        {
            UnPause();
        }
        else
        {
            Pause();
        }
    }

    private void Pause()
    {
        isPaused = true;

        look.HideCursor = false;

        pauseMenu.SetActive(true);

        Time.timeScale = 0;
    }

    public void UnPause()
    {
        isPaused = false;

        look.HideCursor = true;

        pauseMenu.SetActive(false);

        Time.timeScale = 1;
    }
}

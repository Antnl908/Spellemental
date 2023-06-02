using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menu_Interactions : MonoBehaviour
{
    [SerializeField]
    private string levelName = "Colosseum_URP_Scene";

    [SerializeField]
    private string tutorialName = "Tutorial_Scene";

    [SerializeField]
    private GameObject loadingScreen;

    [SerializeField]
    private Slider progressBar;

    [SerializeField]
    private TextMeshProUGUI progressText;
    
    // Start is called before the first frame update
    void Start()
    {
        loadingScreen.SetActive(false);
    }

    /// <summary>
    /// Loads the colosseum.
    /// </summary>
    public void StartLevel()
    {
        Score_Keeper.ResetScore();

        StartCoroutine(LoadLevelAsync(levelName));
    }

    /// <summary>
    /// Loads the tutorial.
    /// </summary>
    public void StartTutorial()
    {
        Score_Keeper.ResetScore();

        StartCoroutine(LoadLevelAsync(tutorialName));
    }

    /// <summary>
    /// Closes the application.
    /// </summary>
    public void Quit()
    {
        Application.Quit();

        Debug.Log("Quit!");
    }

    /// <summary>
    /// Displays the player's high scores on a UI element.
    /// </summary>
    /// <param name="text"></param>
    public void DisplayHighScores(TextMeshProUGUI text)
    {
        Score_Keeper.LoadHighScores();

        text.text = string.Empty;

        for(int i = 0; i < Score_Keeper.ScoreList.HighScores.Length; i++)
        {
            text.text += Score_Keeper.ScoreList.HighScores[i] + "\r\n";
        }
    }

    /// <summary>
    /// Deletes all of the player's scores.
    /// </summary>
    public void DeleteAllScores()
    {
        Score_Keeper.DeleteAllScores();
    }

    /// <summary>
    /// Loads a scene asynchronously.
    /// </summary>
    /// <param name="name">Name of the scene that will be loaded</param>
    /// <returns></returns>
    private IEnumerator LoadLevelAsync(string name)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);

        loadingScreen.SetActive(true);

        while(!operation.isDone)
        {
            float loadProgress = Mathf.Clamp01(operation.progress / 0.9f);

            progressBar.value = loadProgress;

            progressText.text = loadProgress * 100f + "%";

            yield return null;
        }
    }
}

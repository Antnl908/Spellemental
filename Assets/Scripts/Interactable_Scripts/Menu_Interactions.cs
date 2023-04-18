using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Menu_Interactions : MonoBehaviour
{
    [SerializeField]
    private string levelName = "Colosseum_URP_Scene";

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

    public void StartLevel()
    {
        StartCoroutine(LoadLevelAsynchronously(levelName));
    }

    public void Quit()
    {
        Application.Quit();

        Debug.Log("Quit!");
    }

    private IEnumerator LoadLevelAsynchronously(string name)
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

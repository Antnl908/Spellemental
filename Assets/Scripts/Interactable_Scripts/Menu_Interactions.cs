using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Interactions : MonoBehaviour
{
    [SerializeField]
    private string levelName = "Colosseum_URP_Scene";
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLevel()
    {
        SceneManager.LoadScene(levelName);
    }

    public void Quit()
    {
        Application.Quit();

        Debug.Log("Quit!");
    }
}

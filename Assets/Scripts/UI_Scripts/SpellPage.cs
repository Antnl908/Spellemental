using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellPage : MonoBehaviour
{
    [SerializeField] private List<GameObject> elements;
    [SerializeField] private TextMeshProUGUI[] texts;

    /// <summary>
    /// Activates all the objects related to a page
    /// </summary>
    /// <param name="color"></param>
    public void OpenPage(Color color)
    {
        Debug.Log("Open page.");
        gameObject.SetActive(true);
        foreach(TextMeshProUGUI t in texts)
        {
            t.color = color;
        }
        foreach(GameObject element in elements)
        {
            element.SetActive(true);
        }
    }

    /// <summary>
    /// Deactivates all the objects related to a page
    /// </summary>
    public void ClosePage()
    {
        gameObject.SetActive(false);
        foreach (GameObject element in elements)
        {
            element.SetActive(false);
        }
    }
}

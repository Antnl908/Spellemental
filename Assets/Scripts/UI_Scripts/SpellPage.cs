using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellPage : MonoBehaviour
{
    [SerializeField] private List<GameObject> elements;
    [SerializeField] private TextMeshProUGUI[] texts;

    //private void Awake()
    //{
    //    //texts = GetComponentsInChildren<TextMeshProUGUI>();
    //    Debug.Log($"Texts found: {texts.Length}");
    //}

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

    public void ClosePage()
    {
        gameObject.SetActive(false);
        foreach (GameObject element in elements)
        {
            element.SetActive(false);
        }
    }
}

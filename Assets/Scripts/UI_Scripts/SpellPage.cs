using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellPage : MonoBehaviour
{
    [SerializeField] private List<GameObject> elements;

    public void OpenPage()
    {
        foreach(GameObject element in elements)
        {
            element.SetActive(true);
        }
    }

    public void ClosePage()
    {
        foreach(GameObject element in elements)
        {
            element.SetActive(false);
        }
    }
}

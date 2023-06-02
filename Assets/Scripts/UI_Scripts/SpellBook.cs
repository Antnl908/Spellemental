using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellBook : MonoBehaviour
{
    [SerializeField] private Color textColor;
    [SerializeField] private List<SpellPage> pages;
    [SerializeField] private TextMeshProUGUI tmpPage;
    private SpellPage currentPage;
    private int currentIndex;

    private void Start()
    {
        GoToPage(1);
    }

    public void OpenBook()
    {

    }
    
    /// <summary>
    /// Opens a page in the spellbook
    /// </summary>
    /// <param name="index"></param>
    public void GoToPage(int index)
    {
        index--;
        if(index < 0 || index >= pages.Count)
        {
            Debug.Log($"Page: {index + 1} is out of index.");
            return;
        }

        for(int i = 0; i < pages.Count; i++)
        {
            if(i == index) 
            {
                pages[i].OpenPage(textColor); 
                currentIndex = i; 
                currentPage = pages[i];
                Debug.Log("Opening page");

            }
            else 
            { 
                pages[i].ClosePage(); 
            }
        }

        Debug.Log($"Page: {index + 1} at index: {currentIndex}.");
        tmpPage.color = textColor;
        tmpPage.text = $"{index + 1}/{pages.Count}";
    }

    /// <summary>
    /// Update index and opens the next page
    /// </summary>
    public void NextPage()
    {
        if(currentIndex + 1 >= pages.Count) { GoToPage(1); return; }
        GoToPage(currentIndex + 2);
    }

    /// <summary>
    /// Update the index and opens the previous page
    /// </summary>
    public void PreviousPage()
    {
        if (currentIndex  <= 0) { GoToPage(pages.Count); return; }
        GoToPage(currentIndex);
    }
}

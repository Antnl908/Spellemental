using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBook : MonoBehaviour
{
    [SerializeField] private List<SpellPage> pages;
    private SpellPage currentPage;
    private int currentIndex;

    private void Start()
    {
        GoToPage(1);
    }

    public void OpenBook()
    {

    }
    
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
                pages[i].OpenPage(); 
                currentIndex = i; 
                currentPage = pages[i];
            }
            else 
            { 
                pages[i].ClosePage(); 
            }
        }

        Debug.Log($"Page: {index + 1} at index: {currentIndex}.");
    }

    public void NextPage()
    {
        if(currentIndex + 1 >= pages.Count) { GoToPage(1); return; }
        GoToPage(currentIndex + 2);
    }
    public void PreviousPage()
    {
        if (currentIndex  <= 0) { GoToPage(pages.Count); return; }
        GoToPage(currentIndex);
    }
}

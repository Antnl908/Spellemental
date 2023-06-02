using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextCrawl : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField]
    float beginPosition = -580f;
    [SerializeField]
    float endPosition = 5600f;

    [SerializeField] bool debug;
    [SerializeField] float debugPosition;

    RectTransform text;
    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!debug)
        {
            //Update the position of the text and resets if it has scrolled past the screen
            text.anchoredPosition3D += Vector3.up * speed * Time.deltaTime;
            if (text.anchoredPosition3D.y > endPosition) { SetStartPosition(); }
        }
        else
        {
            text.anchoredPosition3D = Vector3.up * debugPosition;
        }
        
    }

    private void OnEnable()
    {
        SetStartPosition();
    }

    /// <summary>
    /// Sets the default position at the bottom of the screen
    /// </summary>
    void SetStartPosition()
    {
        text.anchoredPosition3D = Vector3.up * beginPosition;
    }
}

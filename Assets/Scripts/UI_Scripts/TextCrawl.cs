using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextCrawl : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField]
    float beginPosition = -580f; //-1100; //-2935;
    [SerializeField]
    float endPosition = 5600f; //2935;

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
            //text.localPosition += Vector3.up * speed * Time.deltaTime;
            text.anchoredPosition3D += Vector3.up * speed * Time.deltaTime;
            //text.anchoredPosition3D = Vector3.up * beginPosition;
            if (text.anchoredPosition3D.y > endPosition) { SetStartPosition(); }
        }
        else
        {
            text.anchoredPosition3D = Vector3.up * debugPosition;
        }
        
    }

    private void OnEnable()
    {
        //text.localPosition = Vector3.up * beginPosition;
        //text.localPosition = Vector3.up * -beginPosition;
        //text.anchoredPosition = Vector3.up * beginPosition;
        //text.localPosition = Vector3.up * beginPosition;
        SetStartPosition();
    }

    void SetStartPosition()
    {
        text.anchoredPosition3D = Vector3.up * beginPosition;
    }
}

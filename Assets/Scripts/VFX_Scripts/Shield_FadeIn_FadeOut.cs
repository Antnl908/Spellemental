using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_FadeIn_FadeOut : MonoBehaviour
{
    [SerializeField]
    private Material shieldMaterial;

    private bool fadeIn = false;

    private float currentClipThreshold = 0;

    [SerializeField]
    private float fadeInRate = 10;

    [SerializeField]
    private float fadOutRate = 10;

    private void OnEnable()
    {
        fadeIn = true;

        shieldMaterial.SetFloat("_ClipThreshold", 1);

        currentClipThreshold = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(fadeIn)
        {
            if(currentClipThreshold > 0)
            {
                currentClipThreshold -= fadeInRate * Time.deltaTime;

                shieldMaterial.SetFloat("_ClipThreshold", currentClipThreshold);
            }
        }
        else
        {
            if(currentClipThreshold < 1)
            {
                currentClipThreshold += fadOutRate * Time.deltaTime;

                shieldMaterial.SetFloat("_ClipThreshold", currentClipThreshold);
            }
            else if(currentClipThreshold >= 1)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Deactivate()
    {
        fadeIn = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTime : MonoBehaviour
{
    [SerializeField] MeshRenderer[] rend;
    [SerializeField] bool useUnscaledTime;
    //Material mat;
    void Awake()
    {
        
    }
    /// <summary>
    /// Update shader with unscaled time
    /// </summary>
    void Update()
    {
        if(useUnscaledTime)
        {
            for (int i = 0; i < rend.Length; i++)
            {
                rend[i].material.SetFloat("_UnscaledTime", Time.unscaledTime);
            }
        }
        

        transform.Rotate(0f, 0.2f, 0f);
    }
}

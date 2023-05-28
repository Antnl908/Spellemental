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
        //mat = GetComponent<Material>();
        //mat = rend.material;
    }

    void Update()
    {
        //mat.SetFloat("_UnscaledTime", Time.unscaledTime);
        //Debug.Log("_UnscaledTime is set");
        //if (mat != null) { mat.SetFloat("_UnscaledTime", Time.unscaledTime); } 
        //else
        //{
        //    Debug.Log("Material is null");
        //}
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

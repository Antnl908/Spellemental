using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Play_VFX_On_Start : MonoBehaviour
{
    //Made by Daniel.

    [SerializeField]
    private VisualEffect visualEffect;

    private void OnEnable()
    {
        visualEffect.Play();
    }
}

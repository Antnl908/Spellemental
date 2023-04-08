using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[ExecuteAlways]
public class GiveBurnProperties : MonoBehaviour
{
    //Made by Daniel.

    [SerializeField]
    private Transform effectPosition;

    [SerializeField]
    private float scale = 1.0f;

#nullable enable
    [SerializeField]
    private Material? material;
#nullable disable

    [SerializeField]
    private float noisePower = 1.0f;

    [SerializeField]
    private float noiseSize = 1.0f;

    [SerializeField]
    private Color gizmoColor;

    private VisualEffect effect;

    private Color defaultColor;

    // Start is called before the first frame update
    void Start()
    {
        effect = GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        SetProperties();
    }

    //Sets the properties of VFX and Shader.
    private void SetProperties()
    {
        if(effectPosition && material)
        {
            effectPosition.rotation = new(0, 0, 0, 0);

            effect.SetVector3("Ball Pos", effectPosition.position);
            effect.SetFloat("Ball Size", scale);
            effect.SetFloat("Noise Power", noisePower);
            effect.SetFloat("Noise Size", noiseSize);

            material.SetVector("_Ball_Pos", effectPosition.position);
            material.SetFloat("_Ball_Size", scale);
            material.SetFloat("_Noise_Power", noisePower);
            material.SetFloat("_Noise_Size", noiseSize);

            if(Spell_Casting.HandColor != defaultColor)
            {
                material.SetColor("_Edge_Color", Spell_Casting.HandColor);
            }          
        }
        else if (effectPosition)
        {
            effectPosition.rotation = new(0, 0, 0, 0);

            effect.SetVector3("Ball Pos", effectPosition.position);
            effect.SetFloat("Ball Size", scale);
            effect.SetFloat("Noise Power", noisePower);
            effect.SetFloat("Noise Size", noiseSize);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(effectPosition.position, scale);
    }
}

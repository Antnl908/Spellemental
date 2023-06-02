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

#nullable enable
    [SerializeField] 
    private Transform? shaderEffectPosition;
#nullable disable

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

#nullable enable
    [SerializeField]
    private Spell_Casting? spellCaster;
#nullable disable

    private VisualEffect effect;

    private Color defaultColor;

    [SerializeField]
    private bool isLeftHandEffect = true;

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

    /// <summary>
    /// Sets the properties of the VFX and the Shader.
    /// </summary>
    private void SetProperties()
    {
        if(material)
        {
            if (shaderEffectPosition)
            {
                material.SetVector("_Ball_Pos", shaderEffectPosition.position);
            }
            
            material.SetFloat("_Ball_Size", scale);
            material.SetFloat("_Noise_Power", noisePower);
            material.SetFloat("_Noise_Size", noiseSize);

            if (isLeftHandEffect)
            {
                if(spellCaster != null)
                {
                    if (spellCaster.LeftHandColor() != defaultColor)
                    {
                        material.SetColor("_Edge_Color", spellCaster.LeftHandColor());
                    }
                }                
            }
            else
            {
                if (spellCaster.RightHandColor() != defaultColor)
                {
                    material.SetColor("_Edge_Color", spellCaster.RightHandColor());
                }
            }        
        }
        
        if (effectPosition)
        {
            effectPosition.rotation = new(0, 0, 0, 0);

            effect.SetVector3("Ball Pos", effectPosition.position);
            effect.SetFloat("Ball Size", scale);
            effect.SetFloat("Noise Power", noisePower);
            effect.SetFloat("Noise Size", noiseSize);
        }
    }

    /// <summary>
    /// Draws where the effect and the shader effect are located in the editor.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmoColor;

        if (effectPosition)
        {
            Gizmos.DrawSphere(effectPosition.position, scale);
        }      

        if(shaderEffectPosition)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(shaderEffectPosition.position, scale);
        }
    }
}

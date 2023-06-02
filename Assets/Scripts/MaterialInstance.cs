using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows for instancing of materials while also making it possible to batch them for draw calls 
/// </summary>
public class MaterialInstance : MonoBehaviour
{
    //Made by AntonL, edited by Andreas J

    public Color albedo = Color.white;
    public Color color = Color.white;
    public float amount = 0f;
    public bool glow = false;
    private MaterialPropertyBlock mpb;
    
    private Renderer rend;
    [SerializeField] MeshRenderer[] renderers;
    [SerializeField] SkinnedMeshRenderer[] sRenderers;
    // Start is called before the first frame update
    void Start()
    {
        mpb ??= new MaterialPropertyBlock();
        
        ChangeMatColor();
    }

    /// <summary>
    /// Create new material property block
    /// </summary>
    public void NewMBP()
    {
        mpb = new MaterialPropertyBlock();
        ChangeMatColor();
    }

    public void InitializeMaterial()
    {

        if (renderers.Length > 0)
        {
            for (int i = 0; i < renderers.Length; i++)
            {

            }
        }

        if (sRenderers.Length > 0)
        {
            foreach (SkinnedMeshRenderer r in sRenderers)
            {
                
            }
        }

        ChangeMatColor();
    }

    //Update is called once per frame
    void Update()
    {
        amount -= 0.5f * Time.deltaTime;
        if (amount > 0) { glow = true; } else { glow = false; }
        ChangeMatColor();
    }

    public void SetGlow(bool g)
    {
        glow = g;
        ChangeMatColor();
    }
    public void SetGlow(float a)
    {
        amount = a;
        ChangeMatColor();
    }
    public void SetGlow()
    {
        ChangeMatColor();
    }

    /// <summary>
    /// Sets the values for the material
    /// </summary>
    private void ChangeMatColor()
    {

        mpb.SetFloat("_Amount", amount);
        mpb.SetFloat("_Glow", glow ? 1f : 0f);
        mpb.SetColor("_GlowColor", color);
        mpb.SetColor("_Albedo", albedo);

        if (renderers.Length > 0)
        {
            foreach (MeshRenderer r in renderers)
            {
                r.SetPropertyBlock(mpb);
            }
        }
        

        if(sRenderers.Length > 0)
        {
            foreach (SkinnedMeshRenderer r in sRenderers)
            {
                r.SetPropertyBlock(mpb);
            }
        }
        
    }
    public SkinnedMeshRenderer[] SkinMesh
    {
        set { sRenderers = value; }
    }

    public MeshRenderer[] MeshRenderer
    {
        set { renderers = value; }
    }
}

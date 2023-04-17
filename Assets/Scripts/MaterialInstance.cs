using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialInstance : MonoBehaviour
{
    //Made by AntonL, edited by Andreas J

    public Color albedo = Color.white;
    public Color color;
    public float amount;
    public bool glow;
    private MaterialPropertyBlock mpb;
    private Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        if (mpb == null)
        {
            mpb = new MaterialPropertyBlock();
        }
        //rend = GetComponentInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        amount -=  0.5f * Time.deltaTime;
        SetGlow();
    }

    public void SetGlow(bool g)
    {
        //if (mpb == null)
        //{
        //    mpb = new MaterialPropertyBlock();
        //}

        //Renderer rend = GetComponentInChildren<Renderer>();
        glow = g;
        ChangeMatColor();
    }
    public void SetGlow(float a)
    {
        //if (mpb == null)
        //{
        //    mpb = new MaterialPropertyBlock();
        //}

        //Renderer rend = GetComponentInChildren<Renderer>();
        //glow = g;
        amount = a;
        ChangeMatColor();
    }
    public void SetGlow()
    {
        //if (mpb == null)
        //{
        //    mpb = new MaterialPropertyBlock();
        //}

        //Renderer rend = GetComponentInChildren<Renderer>();
        //glow = g;
        ChangeMatColor();
    }

    private void OnValidate()
    {
        if (mpb == null)
        {
            mpb = new MaterialPropertyBlock();
        }

        ChangeMatColor();
    }

    private void ChangeMatColor()
    {
        MeshRenderer[] renderers = transform.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer r in renderers)
        {
            mpb.SetFloat("_Amount", amount);
            mpb.SetFloat("_Glow", glow ? 1f : 0f);
            mpb.SetColor("_Color", color);
            mpb.SetColor("_Albedo", albedo);
            r.SetPropertyBlock(mpb);
        }

        SkinnedMeshRenderer[] sRenderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (SkinnedMeshRenderer r in sRenderers)
        {
            mpb.SetFloat("_Amount", amount);
            mpb.SetFloat("_Glow", glow ? 1f : 0f);
            mpb.SetColor("_Color", color);
            mpb.SetColor("_Albedo", albedo);
            r.SetPropertyBlock(mpb);
        }
    }
}

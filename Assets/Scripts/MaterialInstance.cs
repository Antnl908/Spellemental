using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialInstance : MonoBehaviour
{
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
        rend = GetComponentInChildren<Renderer>();
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
        mpb.SetFloat("_Amount", amount);
        mpb.SetFloat("_Glow", glow ? 1f : 0f);
        mpb.SetColor("_Color", color);
        rend.SetPropertyBlock(mpb);
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
        mpb.SetFloat("_Amount", amount);
        mpb.SetFloat("_Glow", glow ? 1f : 0f);
        mpb.SetColor("_Color", color);
        rend.SetPropertyBlock(mpb);
    }
    public void SetGlow()
    {
        //if (mpb == null)
        //{
        //    mpb = new MaterialPropertyBlock();
        //}

        //Renderer rend = GetComponentInChildren<Renderer>();
        //glow = g;
        mpb.SetFloat("_Amount", amount);
        mpb.SetFloat("_Glow", glow ? 1f : 0f);
        mpb.SetColor("_Color", color);
        rend.SetPropertyBlock(mpb);
    }

    private void OnValidate()
    {
        if(mpb == null)
        {
            mpb = new MaterialPropertyBlock();
        }

        Renderer rend = GetComponentInChildren<Renderer>();
        mpb.SetFloat("_Amount", amount);
        mpb.SetFloat("_Glow", glow ? 1f : 0f);
        mpb.SetColor("_Color", color);
        rend.SetPropertyBlock(mpb);
    }
}

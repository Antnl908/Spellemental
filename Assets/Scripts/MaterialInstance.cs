using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialInstance : MonoBehaviour
{
    //Made by AntonL, edited by Andreas J

    public Color albedo = Color.white;
    public Color color;
    public float amount = 0f;
    public bool glow = false;
    private MaterialPropertyBlock mpb;
    
    private Renderer rend;
    [SerializeField] MeshRenderer[] renderers;
    [SerializeField] SkinnedMeshRenderer[] sRenderers;
    // Start is called before the first frame update
    void Start()
    {
        if (mpb == null)
        {
            mpb = new MaterialPropertyBlock();
        }
        //rend = GetComponentInChildren<Renderer>();
        //renderers = transform.GetComponentsInChildren<MeshRenderer>();
        //sRenderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    public void NewMBP()
    {
        mpb = new MaterialPropertyBlock();
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
    }

    // Update is called once per frame
    void Update()
    {
        amount -=  0.5f * Time.deltaTime;
        if(amount > 0) { glow = true; } else { glow = false; }
        //SetGlow();
        ChangeMatColor();
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

    //private void OnValidate()
    //{
    //    if (mpb == null)
    //    {
    //        mpb = new MaterialPropertyBlock();
    //    }

    //    ChangeMatColor();
    //}

    private void ChangeMatColor()
    {

        mpb.SetFloat("_Amount", amount);
        mpb.SetFloat("_Glow", glow ? 1f : 0f);
        mpb.SetColor("_Color", color);
        mpb.SetColor("_Albedo", albedo);

        //MeshRenderer[] renderers = transform.GetComponentsInChildren<MeshRenderer>();
        if (renderers.Length > 0)
        {
            foreach (MeshRenderer r in renderers)
            {
                //Debug.Log("Set block");
                //mpb.SetFloat("_Amount", amount);
                //mpb.SetFloat("_Glow", glow ? 1f : 0f);
                //mpb.SetColor("_Color", color);
                //mpb.SetColor("_Albedo", albedo);
                r.SetPropertyBlock(mpb);
            }
        }
        

        //SkinnedMeshRenderer[] sRenderers = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
        if(sRenderers.Length > 0)
        {
            foreach (SkinnedMeshRenderer r in sRenderers)
            {
                //mpb.SetFloat("_Amount", amount);
                //mpb.SetFloat("_Glow", glow ? 1f : 0f);
                //mpb.SetColor("_Color", color);
                //mpb.SetColor("_Albedo", albedo);
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

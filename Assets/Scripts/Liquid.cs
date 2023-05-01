using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Liquid : MonoBehaviour
{
    public enum UpdateMode { Normal, UnscaledTime }
    public UpdateMode updateMode;

    #region Wobble
    [Header("Wobble")]
    [SerializeField] float maxWobble = 0.03f;
    [SerializeField] float wobbleSpeedMove = 1f;
    #endregion
    
    #region Fill
    [Header("Fill")]
    [SerializeField] float fillAmount = 0.5f;
    [SerializeField] float recovery = 1f;
    #endregion 
    
    #region Thickness
    [Header("Fill")]
    [SerializeField] float thickness = 1f;
    [Range(0,1)]
    [SerializeField] public float compensateShapeAmount;
    #endregion
    
    #region Componenets
    [Header("Componenets")]
    [SerializeField] Mesh mesh;
    [SerializeField] Renderer rend;
    #endregion

    Vector3 pos;
    Vector3 lastPos;

    Vector3 velocity;
    Quaternion lastRot;
    Vector3 angularVelocity;

    #region Values
    float wobbleAmountX;
    float wobbleAmountZ;
    float wobbleAmountToAddX;
    float wobbleAmountToAddZ;
    float pulse;
    float sineWave;
    float time = 0.5f;
    Vector3 comp;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        MeshAndRend();
    }

    private void OnValidate()
    {
        MeshAndRend();
    }

    //Update is called once per frame
    void Update()
    {
        float deltaTime = 0;
        switch(updateMode)
        {
            case UpdateMode.Normal:
                deltaTime = Time.deltaTime;
                break;
            case UpdateMode.UnscaledTime:
                deltaTime = Time.unscaledDeltaTime;
                break;
        }

        time += deltaTime;

        if(deltaTime != 0)
        {
            wobbleAmountToAddX = Mathf.Lerp(wobbleAmountToAddX, 0, (deltaTime * recovery));
            wobbleAmountToAddZ = Mathf.Lerp(wobbleAmountToAddZ, 0, (deltaTime * recovery));

            pulse = 2 * Mathf.PI * wobbleSpeedMove;
            sineWave = Mathf.Lerp(sineWave, Mathf.Sin(pulse * time), deltaTime * Mathf.Clamp(velocity.magnitude + angularVelocity.magnitude, thickness, 10));

            wobbleAmountX = wobbleAmountToAddX * sineWave;
            wobbleAmountZ = wobbleAmountToAddZ * sineWave;

            Velocity = (lastPos - transform.position) / deltaTime;
            angularVelocity = GetAngularVelocity(lastRot, transform.rotation);

            wobbleAmountToAddX += Mathf.Clamp((velocity.x + (velocity.y * 0.2f) + angularVelocity.z + angularVelocity.y) * maxWobble, -maxWobble, maxWobble);
            wobbleAmountToAddZ += Mathf.Clamp((velocity.z + (velocity.y * 0.2f) + angularVelocity.x + angularVelocity.y) * maxWobble, -maxWobble, maxWobble);
        }

        rend.sharedMaterial.SetFloat("_WobbleX", wobbleAmountX);
        rend.sharedMaterial.SetFloat("_WobbleZ", wobbleAmountZ);

        UpdatePos(deltaTime);

        lastPos = transform.position;
        lastRot = transform.rotation;
        
    }

    void UpdatePos(float deltaTime)
    {
        Vector3 worldPos = transform.TransformPoint(new Vector3(mesh.bounds.center.x, mesh.bounds.center.y, mesh.bounds.center.z));

        if(compensateShapeAmount > 0)
        {
            if(deltaTime != 0)
            {
                comp = Vector3.Lerp(comp, (worldPos - new Vector3(0, GetLowestPoint(), 0)), deltaTime * 10);
            }
            else
            {
                comp = (worldPos - new Vector3(0, GetLowestPoint(), 0));
            }

            pos = worldPos - transform.position - new Vector3(0, fillAmount - (comp.y * compensateShapeAmount), 0);
        }
        else
        {
            pos = worldPos - transform.position - new Vector3(0,fillAmount,0);
        }

        rend.sharedMaterial.SetVector("_FillAmount", pos);
    }

    void MeshAndRend()
    {
        if (mesh == null) { mesh = GetComponent<MeshFilter>().sharedMesh; }
        if (rend == null) { rend = GetComponent<Renderer>(); }
    }

    Vector3 GetAngularVelocity(Quaternion foreLastFrameRotation, Quaternion lastFrameRotation)
    {
        var q = lastFrameRotation * Quaternion.Inverse(foreLastFrameRotation);

        //if too close to 1 results will be Nan

        if(Mathf.Abs(q.w) > 1023.5f / 1024.0f)
        {
            return Vector3.zero;
        }

        float gain;

        if(q.w < 0.0f)
        {
            var angle = Mathf.Acos(-q.w);
            gain = -2.0f * angle / (Mathf.Sin(angle) * Time.deltaTime);
        }
        else
        {
            var angle = Mathf.Acos(q.w);
            gain = 2.0f * angle / (Mathf.Sin(angle) * Time.deltaTime);
        }

        Vector3 angularVelocity = new Vector3(q.x * gain, q.y * gain, q.z * gain);

        if(float.IsNaN(angularVelocity.z))
        {
            angularVelocity = Vector3.zero;
        }

        return angularVelocity;
    }

    float GetLowestPoint()
    {
        float lowestY = float.MaxValue;
        Vector3 lowestVert = Vector3.zero;
        Vector3[] vertices = mesh.vertices;

        for(int i = 0; i < vertices.Length; i++)
        {
            Vector3 position = transform.TransformPoint(vertices[i]);

            if(position.y < lowestY)
            {
                lowestY = position.y;
                lowestVert = position;
            }
        }

        return lowestVert.y;
    }

    Vector3 Velocity
    {
        get { return velocity; }
        set { velocity = value; }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.VFX;

public class Wind_Hit : Pooling_Object
{
    [SerializeField]
    private VisualEffect effect;

    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private float baseXScaleInner;

    [SerializeField] 
    private float baseXScaleOuter;

    [SerializeField]
    private float baseZScale;

    [SerializeField]
    private float destructionTime = 0.5f;

    private float currentDestructionTime;

    private IObjectPool<Pooling_Object> pool;

    [SerializeField]
    private float baseXRotation = 90f;

    // Update is called once per frame
    void Update()
    {
        currentDestructionTime -= Time.deltaTime;

        if (currentDestructionTime <= 0 && enabled)
        {
            pool.Release(this);
        }
    }

    public override void Initialize(Vector3 position, Quaternion rotation, Vector3 direction, IObjectPool<Pooling_Object> pool)
    {
        currentDestructionTime = destructionTime;

        this.pool = pool;

        spawnPoint.position = position;

        float distance = Vector3.Distance(spawnPoint.position, direction);

        Quaternion windRotation = Quaternion.FromToRotation(direction.normalized, position.normalized);

        effect.SetVector3("Inner Scale", new Vector3(baseXScaleInner, distance, baseZScale));
        effect.SetVector3("Outer Scale", new Vector3(baseXScaleOuter, distance, baseZScale));
        effect.SetVector3("Angle", new Vector3(baseXRotation, 0, windRotation.eulerAngles.z));
    }
}

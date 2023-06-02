using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.VFX;

public class Pooled_VFX : Pooling_Object
{
    [SerializeField]
    private VisualEffect visualEffect;

    [SerializeField]
    private float destructionTime = 0.5f;

    private float currentDestructiontime;

    private IObjectPool<Pooling_Object> pool;

    // Update is called once per frame
    void Update()
    {
        currentDestructiontime -= Time.deltaTime;

        if (currentDestructiontime <= 0 && enabled)
        {
            pool.Release(this);
        }
    }

    /// <summary>
    /// Gives the vfx its starting values
    /// </summary>
    /// <param name="position">The position it is spawned at</param>
    /// <param name="rotation">The rotation it is given</param>
    /// <param name="direction">Unused</param>
    /// <param name="pool">The pool it came from</param>
    public override void Initialize(Vector3 position, Quaternion rotation, Vector3 direction, IObjectPool<Pooling_Object> pool)
    {
        this.pool = pool;

        currentDestructiontime = destructionTime;
        
        transform.SetPositionAndRotation(position, rotation);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Pool;
using System.Runtime.CompilerServices;

public class Damage_Indicator : Pooling_Object
{
    //Made by Daniel.

    private Rigidbody rb;

    private TextMeshProUGUI textMesh;

    private IObjectPool<Pooling_Object> pool;

    private float destructionTime;

    [SerializeField]
    private GameObject child;

    /// <summary>
    /// Displays how much damage an enemy took. Shoots the damage indicator upwards.
    /// </summary>
    /// <param name="damage">How much damage the enemy took</param>
    /// <param name="force">How much force the damage indicator will be shot upwards with</param>
    /// <param name="pool">The pool this object came from</param>
    /// <param name="destructionTime">How long it will be until the object is returned from the pool</param>
    /// <param name="position">The start position of the object</param>
    /// <param name="rotation">The starting rotation of the object</param>
    public void SetValues(int damage, float force, IObjectPool<Pooling_Object> pool, float destructionTime, Vector3 position, Quaternion rotation)
    {
        if(rb == null)
        {
            rb = GetComponentInChildren<Rigidbody>();
        }

        rb.velocity = Vector3.zero;

        if (textMesh == null)
        {
            textMesh = GetComponentInChildren<TextMeshProUGUI>();
        }      

        this.pool = pool;
        this.destructionTime = destructionTime;

        transform.SetPositionAndRotation(position, rotation);

        child.transform.SetPositionAndRotation(position, rotation);
        
        textMesh.text = damage.ToString();

        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
    }

    /// <summary>
    /// Releases the object back to the pool after a certain amount of time.
    /// </summary>
    private void Update()
    {
        destructionTime -= Time.deltaTime;

        if(destructionTime < 0 )
        {
            pool.Release(this);
        }
    }
}

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

    //Sets the values so it displays correctly.
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

    private void Update()
    {
        destructionTime -= Time.deltaTime;

        if(destructionTime < 0 )
        {
            pool.Release(this);
        }
    }
}

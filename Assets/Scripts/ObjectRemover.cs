using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRemover : MonoBehaviour
{
    Rigidbody[] RB;
    [SerializeField] private float power;
    [SerializeField] private float radius;
    [SerializeField] private float upwardForce;

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponentsInChildren<Rigidbody>();
        foreach(var rb in RB) 
        {
            rb.AddExplosionForce(power, transform.position, radius, upwardForce);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveObject()
    {
        Destroy(gameObject);
    }
}

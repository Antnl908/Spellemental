using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Add_Downward_Force : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private float force = 2f;

    /// <summary>
    /// Shoots the object downwards.
    /// </summary>
    private void OnEnable()
    {
        rb.velocity = Vector3.zero;

        rb.AddForce(Physics.gravity * force, ForceMode.Acceleration);
    }
}

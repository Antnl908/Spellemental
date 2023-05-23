using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] rigidbodies;
    Rigidbody rigidBody;
    Animator anim;
    bool isActivated { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        rigidBody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        DeactivateRagdoll();
    }

    public void ActivateRagdoll()
    {
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = false;
            
        }
        rigidBody.isKinematic = true;
        anim.enabled = false;
        isActivated = true;
    }

    public void DeactivateRagdoll()
    {
        foreach(var rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = true;
        }
        rigidBody.isKinematic = true;
        anim.enabled = true;
        isActivated = false;
    }

    public void ApplyForce(Vector3 vector, float force)
    {
        
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.AddForce(vector * force, ForceMode.Impulse);

        }
    }
    public bool IsActivated => isActivated;
}

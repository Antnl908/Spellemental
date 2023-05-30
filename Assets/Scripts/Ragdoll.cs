using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] rigidbodies;
    Rigidbody rigidBody;
    Animator anim;
    bool isActivated { get; set; }

    [SerializeField]
    Transform body;

    [SerializeField]
    float range = 5f;
    
    [SerializeField]
    bool useOverrideRagdol = false;
    [SerializeField]
    Rigidbody overrideBody;
    [SerializeField]
    Collider overrideCollider;


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
        if(useOverrideRagdol)
        {
            rigidBody.isKinematic = true;
            overrideBody.isKinematic = false;
            overrideCollider.enabled = true;
            anim.enabled = false;
            isActivated = true;
            return;
        }

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
        if (useOverrideRagdol)
        {
            rigidBody.isKinematic = true;
            overrideBody.isKinematic = true;
            overrideCollider.enabled = false;
            anim.enabled = true;
            isActivated = false;
            return;
        }

        foreach (var rigidbody in rigidbodies)
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

    public bool AdjustPosition()
    {
        if(body == null)
        {
            return false;
        }
        
        Vector3 adjustedPosition;
        if(RandomPoint(out adjustedPosition))
        {
            transform.position = adjustedPosition;
            return true;
        }
        return false;
    }

    bool RandomPoint(out Vector3 result)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(body.position, out hit, range, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }
        result = Vector3.zero;
        return false;
    }


}

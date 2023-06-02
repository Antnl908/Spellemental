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

    /// <summary>
    /// used to activate ragdoll for an object
    /// </summary>
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

    /// <summary>
    /// Used to deactivate ragdoll
    /// </summary>
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

    /// <summary>
    /// When ragdoll is activated it needs a direction and force to where it should fly, which applies here
    /// </summary>
    /// <param name="vector">the direction that they fly</param>
    /// <param name="force">the force at which they are launched</param>
    public void ApplyForce(Vector3 vector, float force)
    {
        
        foreach (var rigidbody in rigidbodies)
        {
            rigidbody.AddForce(vector * force, ForceMode.Impulse);

        }
    }
    public bool IsActivated => isActivated;

    /// <summary>
    /// Adjusts the position after ragdoll ends
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Finds closest point on the navmesh to where the ragdoll landed. If there is no point close enough, the object is destroyed.
    /// </summary>
    /// <param name="result">the adjusted position</param>
    /// <returns></returns>
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

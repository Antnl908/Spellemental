using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoRagdoll : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float range = 3;

    [SerializeField]
    private LayerMask enemyMask;

    
    float timer;

    [SerializeField]
    float delay = 1f;

    [SerializeField]
    float force = 10f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0 ) 
        { 
            CheckHits(); 
            timer = delay; 
        }
    }

    /// <summary>
    /// Checks to see if the tornado has hit something.
    /// </summary>
    private void CheckHits()
    {
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, range, enemyMask);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                
                Ragdoll ragdoll = colliders[i].transform.GetComponent<Ragdoll>();

                

                if (ragdoll != null)
                {
                    if(ragdoll.IsActivated)
                    {
                        continue;
                    }
                    ragdoll.ActivateRagdoll();
                    Vector3 forceDir = colliders[i].transform.position - transform.position;
                    forceDir.Normalize();
                    ragdoll.ApplyForce(forceDir, force);
                }

                
            }
        }

        
    }

    /// <summary>
    /// Draws in editor
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

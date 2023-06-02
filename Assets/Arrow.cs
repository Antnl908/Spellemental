using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Arrow : MonoBehaviour
{
    private int damage = 100;

    public float lifeTime;
    public float routineDelay;
    public float projectileSpeed;

    [Range(0.0f, 1.0f)]
    public float radius;
    

    private Rigidbody rb;

    public LayerMask destroyOnContactLayer;

    private SphereCollider col;

    /// <summary>
    /// Upon creation sets the velocity and rotation of the projectile, along with what layers should be ignored
    /// </summary>
    private void Start()
    {
        Destroy(gameObject, lifeTime);

        col = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * projectileSpeed;

        Physics.IgnoreLayerCollision(0,7);
        Physics.IgnoreLayerCollision(0, 12);
        Physics.IgnoreLayerCollision(0, 0);
        //StartCoroutine(CollisionCheckRoutine());
    }

    /*
    private IEnumerator CollisionCheckRoutine()
    {
        WaitForSeconds waitTimer = new WaitForSeconds(routineDelay);

        while (true)
        {
            yield return waitTimer;

            CollisionCheck();
        }
    }
    */

    /*
    private void CollisionCheck()
    {
        Collider[] collidersCheck = Physics.OverlapSphere(gameObject.transform.position, 0.002f, destroyOnContactLayer);

        for (int i = 0; i < collidersCheck.Length; i++)
        {
            if (collidersCheck[i].gameObject != gameObject)
            {
                IDamageable damagable = collidersCheck[i].transform.GetComponent<IDamageable>();

                damagable?.TryToDestroyDamageable(damage, null);

                Destroy(gameObject);
            }
        }
    }
    */

    /// <summary>
    /// When colliding, try to deal damage then destroy
    /// </summary>
    private void OnCollisionEnter(Collision col)
    {
        Collider[] collidersCheck = Physics.OverlapSphere(gameObject.transform.position, radius, destroyOnContactLayer);

            for (int i = 0; i < collidersCheck.Length; i++)
            {
                if (collidersCheck[i].gameObject != gameObject)
                {
                    IDamageable damagable = collidersCheck[i].transform.GetComponent<IDamageable>();

                    damagable?.TryToDestroyDamageable(damage, null);

                    Destroy(gameObject);
                }
            }

        //Destroy(gameObject);
    }
    

    private void OnDrawGizmosSelected()
    {
        if (gameObject == null) 
            return;

        Gizmos.DrawWireSphere(gameObject.transform.position, radius);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private AIConfig config;

    [SerializeField] private UnityEngine.AI.NavMeshAgent navAgent;

    [SerializeField]
    private int damage = 1;

    [SerializeField]
    private Transform attackPoint;

    [SerializeField]
    private float attackRadius = 1f;

    /*
    [Header("Target Temporary/Debug")]
    [SerializeField] private Transform target;
    */

    void Start()
    {
        NavAgent.speed = Config.speed;
        /*
        meshes = gameObject.GetComponentsInChildren(typeof(SkinnedMeshRenderer));
        foreach (SkinnedMeshRenderer component in meshes)
        {
            component.material = material;
        }

        meshes = gameObject.GetComponentsInChildren(typeof(MeshRenderer));
        foreach (MeshRenderer component in meshes)
        {
            component.material = material;
        }
        */
    }

    public NavMeshAgent NavAgent
    {
        get
        {
            if (navAgent == null) { var n = GetComponent<UnityEngine.AI.NavMeshAgent>(); if (n != null) { navAgent = n; } else { navAgent = gameObject.AddComponent<UnityEngine.AI.NavMeshAgent>(); } }
            return navAgent;
        }
    }

    public AIConfig Config
    {
        get { return config; }
    }

    public void Attack()
    {
        Collider[] colliders = Physics.OverlapSphere(attackPoint.position, attackRadius);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                IDamageable damagable = colliders[i].transform.GetComponent<IDamageable>();

                damagable?.TryToDestroyDamageable(damage, null);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) { return; }
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}

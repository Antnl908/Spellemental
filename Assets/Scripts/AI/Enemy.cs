using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private AIConfig config;

    [SerializeField] private UnityEngine.AI.NavMeshAgent navAgent;

    //[SerializeField] private Material material; 

    private Component[] meshes;

    /*
    [Header("Target Temporary/Debug")]
    [SerializeField] private Transform target;
    */

    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {

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

    
}

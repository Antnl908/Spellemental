using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField] private AIConfig config;

    [Header("NavMesh")]
    [SerializeField] private UnityEngine.AI.NavMeshAgent navAgent;

    /*
    [Header("Target Temporary/Debug")]
    [SerializeField] private Transform target;
    */

    // Start is called before the first frame update
    void Start()
    {
        NavAgent.speed = Config.speed;
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

    /*
    public Transform Target
    {
        get { return target; }
    }
    */

    public AIConfig Config
    {
        get { return config; }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Anton L
//Based a tutorial by KiwiCoder
//https://www.youtube.com/watch?v=1H9jrKyWKs0&t=974s

/// <summary>
/// Unused StateMachine
/// </summary>
public class Agent : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField] private AIConfig config;

    [Header("State Machine")]
    [HideInInspector] public StateMachine stateMachine;
    [SerializeField] private StateId entryState;
    [SerializeField] private NavMeshAgent navAgent;

    [Header("Target Temporary/Debug")]
    [SerializeField] private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        stateMachine = new StateMachine(this);
        stateMachine.RegisterState(new ChaseState());
        stateMachine.RegisterState(new DeathState());
        stateMachine.ChangeState(entryState);

        NavAgent.speed = Config.speed;
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }

    public NavMeshAgent NavAgent
    {
        get 
        {
            if(navAgent == null) { var n = GetComponent<NavMeshAgent>(); if (n != null) { navAgent = n; } else { navAgent = gameObject.AddComponent<NavMeshAgent>(); } }
            return navAgent;
        }
    }

    public Transform Target
    {
        get { return target; }
    }

    public AIConfig Config
    {
        get { return config; }
    }
}

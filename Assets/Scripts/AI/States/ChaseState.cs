using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    public StateId GetId()
    {
        return StateId.Chase;
    }
    public void Enter(Agent agent)
    {
        Debug.Log("Enter Chase State");
    }
    public void Update(Agent agent)
    {
        Debug.Log("Update Chase State");
        //Do logic here
        if(agent.Target == null) { return; }

        agent.NavAgent.SetDestination(agent.Target.position);

        
    }
    public void LateUpdate(Agent agent)
    {
        Debug.Log("LateUpdate Chase State");
    }
    public void FixedUpdate(Agent agent)
    {
        Debug.Log("FixedUpdate Chase State");
    }
    public void Exit(Agent agent)
    {
        Debug.Log("Exit Chase State");
    }
}

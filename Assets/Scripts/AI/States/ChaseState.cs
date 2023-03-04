using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{
    float timer;
    //float updateTime = 3f;
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
        //Do logic here
        if(agent.Target == null) { return; }

        timer += Time.deltaTime;
        if(timer > agent.Config.chaseUpdateTime)
        {
            Debug.Log("Update Chase State");
            agent.NavAgent.SetDestination(agent.Target.position);
            timer = 0f;
        }


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

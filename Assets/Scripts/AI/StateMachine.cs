using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Anton L
//Based a tutorial by KiwiCoder
//https://www.youtube.com/watch?v=1H9jrKyWKs0&t=974s

/// <summary>
/// Unused StateMachine
/// </summary>
public class StateMachine
{
    public State[] states;
    public StateId currentState;
    public Agent agent;

    public StateMachine(Agent agent)
    {
        this.agent = agent;
        int numStates = System.Enum.GetNames(typeof(StateId)).Length;
        states = new State[numStates];
    }

    public void RegisterState(State state)
    {
        int index = (int)state.GetId();
        states[index] = state;
    }

    public State GetState(StateId stateId)
    {
        int index = (int)stateId;
        return states[index];
    }

    public void Update()
    {
        GetState(currentState)?.Update(agent);
    }
    public void LateUpdate()
    {
        GetState(currentState)?.LateUpdate(agent);
    }
    public void FixedUpdate()
    {
        GetState(currentState)?.FixedUpdate(agent);
    }

    public void ChangeState(StateId newState)
    {
        GetState(currentState)?.Exit(agent);
        currentState = newState;
        GetState(currentState)?.Enter(agent);
        //agent.currentState = currentState;
    }
}

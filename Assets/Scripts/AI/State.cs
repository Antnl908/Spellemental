using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Anton L
//Based a tutorial by KiwiCoder
//https://www.youtube.com/watch?v=1H9jrKyWKs0&t=974s

/// <summary>
/// Unused StateMachine
/// </summary>
public enum StateId
{
    Chase,
    Death
}
public interface State
{
    StateId GetId();
    void Enter(Agent agent);
    void Update(Agent agent);
    void LateUpdate(Agent agent);
    void FixedUpdate(Agent agent);
    void Exit(Agent agent);

}
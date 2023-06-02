using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindWaypoint : StateMachineBehaviour
{
    private Enemy enemy;
    private Bat bat;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Enemy>();
        bat = animator.GetComponent<Bat>();

        Transform[] positions = bat.WayPoints.GetComponentsInChildren<Transform>();
        SetNewWayPoint(positions);
        //animator.transform.LookAt(bat.ActiveWaypoint);
    }

    /// <summary>
    /// Sets a new random target position from a set of waypoints
    /// </summary>
    /// <param name="positions">Transforms from a list of emptyobjects</param>
    private void SetNewWayPoint(Transform[] positions)
    {
        int randomWaypoint = Random.Range(0, positions.Length);
        bat.ActiveWaypoint = positions[randomWaypoint].transform.position;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}

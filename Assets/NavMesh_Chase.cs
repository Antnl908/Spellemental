using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMesh_Chase : StateMachineBehaviour
{
    float timer;
    Enemy enemy;

    Transform player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = animator.GetComponent<Enemy>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, animator.GetComponent<Transform>().position) > 5)
        {
            Debug.Log("Set Chase to Idle State");
            animator.SetTrigger("Idle");

        }

        timer += Time.deltaTime;
        if (timer > enemy.Config.chaseUpdateTime)
        {
            Debug.Log("Update Chase State");
            animator.GetComponent<Enemy>().NavAgent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
            timer = 0f;
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Idle");
    }

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

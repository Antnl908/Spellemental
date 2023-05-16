using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    Transform player;
    Enemy enemy;
    //float timer;

    Animator[] bowAnimator;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = animator.GetComponent<Enemy>();

        animator.GetComponent<Enemy>().NavAgent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
        animator.transform.LookAt(player);
        enemy.NavAgent.speed = 0f;
        */

        bowAnimator = animator.GetComponentsInChildren<Animator>();

        foreach (Animator bow in bowAnimator)
            bow.SetTrigger("DrawString");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*
        //if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, animator.GetComponent<Transform>().position) > 1f)  //sparar denna
        if (Vector3.Distance(player.position, enemy.transform.position) > 1f)
        {
            //Debug.Log("Set Chase to Attack State");
            animator.SetTrigger("Chase");
        }
        else
        {
            animator.ResetTrigger("Chase");
        }
        */
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        /*
        animator.ResetTrigger("Chase");

        enemy.NavAgent.speed = enemy.Config.speed;
        */
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

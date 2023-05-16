using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherChaseState : StateMachineBehaviour
{
    float timer;
    Enemy enemy;

    Transform player;

    Animator[] bowAnimator;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = animator.GetComponent<Enemy>();
        bowAnimator = animator.GetComponentsInChildren<Animator>();

        foreach (Animator bow in bowAnimator)
            bow.SetTrigger("DrawString");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float dist = Vector3.Distance(player.position, enemy.transform.position);

        if (dist > enemy.Config.aggroMaxRange)
        {
            animator.SetTrigger("Idle");
        }
        else if (dist < 1.5f)
        {
            animator.SetTrigger("Attack");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //bowAnimator.SetTrigger("DrawString");

        animator.SetTrigger("Idle");
        animator.SetTrigger("Attack");
        //animator.SetTrigger("DrawString");
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

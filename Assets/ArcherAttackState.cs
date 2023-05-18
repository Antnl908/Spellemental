using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAttackState : StateMachineBehaviour
{
    private Animator bowAnimator;

    private Transform player;
    private Enemy enemy;
    private FieldOfView fov;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bowAnimator = FindFirstObjectByType<Animator>();
        bowAnimator.SetTrigger("DrawString");

        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = animator.GetComponent<Enemy>();
        fov = animator.GetComponent<FieldOfView>();

        enemy.NavAgent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
        enemy.transform.LookAt(player);
        enemy.NavAgent.speed = 0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!fov.canSeePlayer)
        {
            animator.SetTrigger("Chase");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bowAnimator.ResetTrigger("DrawString");
        animator.ResetTrigger("Chase");

        if (!fov.canSeePlayer)
        {
            enemy.NavAgent.speed = enemy.Config.speed;
        }
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

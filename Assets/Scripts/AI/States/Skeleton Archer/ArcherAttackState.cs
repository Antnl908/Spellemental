using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAttackState : StateMachineBehaviour
{
    private Animator bowAnimator;

    private Transform player;
    private Enemy enemy;
    private FieldOfView fov;

    /// <summary>
    /// Upon entering the state: the bow draw animation is triggered, the enemy is set to look at the player and speed is stopped.
    /// </summary>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bowAnimator = animator.gameObject.GetComponentsInChildren<Animator>()[1];
        bowAnimator.SetTrigger("DrawString");

        //player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = animator.GetComponent<Enemy>();
        fov = animator.GetComponent<FieldOfView>();

        enemy.NavAgent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
        enemy.transform.LookAt(player);
        enemy.NavAgent.speed = 0f;
    }

    /// <summary>
    /// Checks if the enemy can no longer see the player
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!fov.canSeePlayer)
        {
            animator.SetTrigger("Chase");
        }
    }

    /// <summary>
    /// Upon exit set normal speed
    /// </summary>
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bowAnimator.ResetTrigger("DrawString");
        animator.ResetTrigger("Chase");

        if (!fov.canSeePlayer)
        {
            enemy.NavAgent.speed = enemy.Config.speed;
        }
    }
}

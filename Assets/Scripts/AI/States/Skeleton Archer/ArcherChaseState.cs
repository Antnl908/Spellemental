using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherChaseState : StateMachineBehaviour
{
    private float timer;
    private Enemy enemy;

    private Transform player;
    private FieldOfView fov;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = animator.GetComponent<Enemy>();
        fov = animator.GetComponent<FieldOfView>();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if (timer > enemy.Config.chaseUpdateTime)
        {
            enemy.NavAgent.SetDestination(player.position);
            timer = 0f;

            float dist = Vector3.Distance(player.position, enemy.transform.position);

            if (dist > enemy.Config.aggroMaxRange)
            {
                animator.SetTrigger("Idle");
            }
            else if (fov.canSeePlayer)
            {
                animator.SetTrigger("Attack");
            }
        }       
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Attack");
    }
}

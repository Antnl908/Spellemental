using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase_State : StateMachineBehaviour
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

    /// <summary>
    /// Changes to different states depending on distance. 
    /// </summary>
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

        timer += Time.deltaTime;
        if (timer > enemy.Config.chaseUpdateTime)
        {
            //Debug.Log("Update Chase State");
            //animator.GetComponent<Enemy>().NavAgent.SetDestination(GameObject.FindGameObjectWithTag("Player").transform.position);
            SetTargetPosition();

            timer = 0f;
        }

    }

    /// <summary>
    /// Sets the target position for the enemy navagent
    /// </summary>
    private void SetTargetPosition()
    {
        if (enemy.NavAgent != null)
        {
            if (enemy.NavAgent.isActiveAndEnabled && enemy.NavAgent.isOnNavMesh)
            {
                enemy.NavAgent.SetDestination(player.position);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Idle");
        animator.ResetTrigger("Attack");
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

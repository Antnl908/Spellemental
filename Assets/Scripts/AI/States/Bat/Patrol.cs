using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : StateMachineBehaviour
{
    private Enemy enemy;
    private Bat bat;
    private FieldOfView fov;

    private float timer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Enemy>();
        bat = animator.GetComponent<Bat>();
        fov = animator.GetComponent<FieldOfView>();
    }

    /// <summary>
    /// If the enemy can see the player enter "Chase" state else if within distance of a waypoint find a new one
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.transform.Translate(Vector3.forward * 5f/*enemy.Config.speed*/ * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer > enemy.Config.chaseUpdateTime)
        {
            if (fov.canSeePlayer)
            {
                animator.SetTrigger("Chase");
            }

            if (Vector3.Distance(bat.ActiveWaypoint, animator.transform.position) < 10f)
            {
                animator.SetTrigger("FindNewWaypoint");
            }
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Chase");
        animator.ResetTrigger("FindNewWaypoint");
    }
}
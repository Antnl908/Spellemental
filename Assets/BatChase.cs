using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatChase : StateMachineBehaviour
{
    private Enemy enemy;
    private Bat bat;

    private float timer;
    private Transform player;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Enemy>();
        bat = animator.GetComponent<Bat>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator.transform.LookAt(player.position);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.Translate(Vector3.forward * 5f/*enemy.Config.speed*/ * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer > enemy.Config.chaseUpdateTime)
        {
            Vector3 followPosition = new Vector3(player.position.x, player.position.y + -1f, player.position.z);

            animator.transform.LookAt(followPosition);

            if(Vector3.Distance(animator.transform.position, followPosition) < 6f)
            {
                bat.ActiveWaypoint = followPosition;

                animator.SetTrigger("Attack");
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
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

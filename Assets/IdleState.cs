using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : StateMachineBehaviour
{
    float timer;
    Transform player;
    Enemy enemy;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = animator.GetComponent<Enemy>();

        enemy.NavAgent.speed = 0f;
    }

    /// <summary>
    /// If player is within aggro range enter "Chase state"
    /// </summary>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //Debug.Log(Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, animator.GetComponent<Transform>().position));

        timer += Time.deltaTime;
        if (timer > enemy.Config.idleUpdateTime)
        {
            //if (Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, animator.GetComponent<Transform>().position) < enemy.Config.aggroMinRange)  //sparar denna till vidare
            if (Vector3.Distance(player.position, enemy.transform.position) < enemy.Config.aggroMinRange)
            {
                //Debug.Log("Set Idle to Chase State");
                animator.SetTrigger("Chase");
            }
        }
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Chase");

        enemy.NavAgent.speed = enemy.Config.speed;
    }
}

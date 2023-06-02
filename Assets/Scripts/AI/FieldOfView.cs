using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;


public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    public float routineDelay;

    public GameObject player;

    public LayerMask target;
    public LayerMask obstructions;

    public bool angledDown;

    public bool canSeePlayer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    /// <summary>
    /// A routine for how often the gameobject should run the method
    /// </summary>
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds waitTimer = new WaitForSeconds(routineDelay);

        while (true)
        {
            yield return waitTimer;

            FielOfViewCheck();
        }
    }

    /// <summary>
    /// Uses raycasts to determine if the target is infront or below the gameobject
    /// </summary> 
    private void FielOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, target);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;

            Vector3 directionToTarget = (target.position - transform.position).normalized;

            float castAngle;

            if (!angledDown)
                castAngle = Vector3.Angle(transform.forward, directionToTarget);
            else
                castAngle = Vector3.Angle(-transform.up, directionToTarget);

            if (castAngle < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructions))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }

    /// <summary>
    /// Creates a gizmo in the editor to visualize the line of sight of the gameobject, including angle and if the gameobject has visual on the target
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.white;
        if (!angledDown)
            Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, radius);
        else
            Handles.DrawWireArc(transform.position, Vector3.forward, Vector3.left, 360, radius);

        Vector3 viewAngle1;
        Vector3 viewAngle2;

        if (!angledDown)
        {
            viewAngle1 = DirectionFromAngle(transform.eulerAngles.y, -angle / 2);
            viewAngle2 = DirectionFromAngle(transform.eulerAngles.y, angle / 2);
        }
        else
        {
            viewAngle1 = DirectionFromAngle(transform.eulerAngles.x, -angle / 2);
            viewAngle2 = DirectionFromAngle(transform.eulerAngles.x, angle / 2);
        }
            

        Handles.color = Color.yellow;
        Handles.DrawLine(transform.position, transform.position + viewAngle1 * radius);
        Handles.DrawLine(transform.position, transform.position + viewAngle2 * radius);

        if (canSeePlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(transform.position, player.transform.position);
        }
    }

    /// <summary>
    /// Retrives the direction from the viewing angle and rotation of the gameobject
    /// </summary>
    /// <param name="euler">Rotation</param>
    /// <param name="angleInDegrees">Angle</param>
    /// <returns>A direction represented by a Vector3</returns>
    private Vector3 DirectionFromAngle(float euler, float angleInDegrees)
    {
        angleInDegrees += euler;

        if (!angledDown)
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        else
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), -Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);

    }
}

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

    public bool canSeePlayer;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds waitTimer = new WaitForSeconds(routineDelay);

        while (true)
        {
            yield return waitTimer;

            FielOfViewCheck();
        }
    }

    private void FielOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, target);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;

            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
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

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.white;
        Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, radius);

        Vector3 viewAngle1 = DirectionFromAngle(transform.eulerAngles.y, -angle / 2);
        Vector3 viewAngle2 = DirectionFromAngle(transform.eulerAngles.y, angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(transform.position, transform.position + viewAngle1 * radius);
        Handles.DrawLine(transform.position, transform.position + viewAngle2 * radius);

        if (canSeePlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(transform.position, player.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}

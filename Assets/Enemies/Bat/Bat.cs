using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Bat : MonoBehaviour
{
    private Enemy_Health enemy;

    [SerializeField]
    private GameObject waypoints;

    private Vector3 activeWaypoint;

    [SerializeField]
    private float speed;
    [Range(0.0f, 4.0f)]
    public float turnSpeed;
    private float angleTurnDegree;

    private Vector3 direction;
    private Quaternion lookRotation;

    public GameObject WayPoints
    {
        get { return waypoints; }
    }

    public Vector3 ActiveWaypoint
    {
        set { activeWaypoint = value; }
        get { return activeWaypoint; }
    }

    private void Start()
    {
        enemy = GetComponent<Enemy_Health>();
    }

    private void Update()
    {
        if (!enemy.isDead)
        {
            direction = (activeWaypoint - transform.position).normalized;
            lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
}

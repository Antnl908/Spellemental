using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Bat : MonoBehaviour
{
    [SerializeField]
    private GameObject waypoints;

    private Vector3 activeWaypoint;

    public GameObject WayPoints
    {
        get { return waypoints; }
    }

    public Vector3 ActiveWaypoint
    {
        set { activeWaypoint = value; }
        get { return activeWaypoint; }
    }
}

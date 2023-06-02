using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorIcons : MonoBehaviour
{
    /// <summary>
    /// Draws a sphere where the waypoints are located in editor
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Transform[] waypoints = GetComponentsInChildren<Transform>();

        Gizmos.color = Color.green;
        foreach(Transform waypoint in waypoints)
        {
            Gizmos.DrawWireSphere(waypoint.position, 1);
        }
    }
}

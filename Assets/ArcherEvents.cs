using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherEvents : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;

    [SerializeField]
    private Arrow arrow;

    [SerializeField]
    private Transform arrowRefPos;

    private Transform playerPos;
    private Vector3 targetPosition;

    private void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Hide_Arrow()
    {
        mesh.enabled = false;
    }

    public void Show_Arrow()
    {
        mesh.enabled = true;
    }

    public void SetTargetPosition()
    {
        targetPosition = playerPos.position;
    }

    public void FireArrow()
    {
        Vector3 direction = targetPosition - arrowRefPos.position;

        Instantiate(arrow, arrowRefPos.position, Quaternion.LookRotation(direction, Vector3.up));

        //Instantiate(arrow, arrowRefPos.position, 
           // Quaternion.identity);
    }
}
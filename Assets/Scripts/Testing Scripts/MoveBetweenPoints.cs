using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBetweenPoints : MonoBehaviour
{
    [SerializeField]
    private Transform pointA;

    [SerializeField]
    private Transform pointB;

    [SerializeField]
    private int speed = 100;

    private bool isMovingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingRight)
        {
            if (transform.position.x < pointA.position.x)
            {
                transform.position += Vector3.right * Time.deltaTime * speed;
            }
            else
            {
                isMovingRight = false;
            }
        }
        else
        {
            if (transform.position.x > pointB.position.x)
            {
                transform.position -= Vector3.right * Time.deltaTime * speed;
            }
            else
            {
                isMovingRight = true;
            }
        }
    }
}

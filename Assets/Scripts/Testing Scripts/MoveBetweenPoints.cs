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

    [SerializeField]
    private bool isUsingForce = false;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingRight)
        {
            if (transform.position.x < pointA.position.x)
            {
                if(isUsingForce)
                {
                    rb.AddForce(speed * Time.deltaTime * Vector3.right);
                }
                else
                {
                    transform.position += speed * Time.deltaTime * Vector3.right;
                }               
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
                if (isUsingForce)
                {
                    rb.AddForce(-speed * Time.deltaTime * Vector3.right);
                }
                else
                {
                    transform.position -= speed * Time.deltaTime * Vector3.right;
                }
            }
            else
            {
                isMovingRight = true;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    Vector3 velocity;
    Vector3 angularVelocity;
    Vector3 lastPos;
    Vector3 lastRot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    //void Update()
    //{
    //    Velocity = (lastPos - transform.position) / Time.deltaTime;
    //    angularVelocity = GetAngularVelocity(lastRot, transform.rotation);
    //}

    Vector3 Velocity
    {
        get { return velocity; }
        set { velocity = value; }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationOverride : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //transform.rotation = Quaternion.FromToRotation(transform.up, Vector3.up);
        transform.up = Vector3.up;
    }
}

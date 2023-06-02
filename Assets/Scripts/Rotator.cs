using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] Vector3 rotationAxis;
    void Update()
    {
        //Rotates object using a Vector3
        transform.rotation *= Quaternion.Euler(rotationAxis.x * Time.unscaledDeltaTime, rotationAxis.y * Time.unscaledDeltaTime, rotationAxis.z * Time.unscaledDeltaTime);
    }
}

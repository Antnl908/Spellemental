using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] Vector3 rotationAxis;
    void Update()
    {
        //transform.Rotate(rotationAxis.x * Time.deltaTime, rotationAxis.y * Time.deltaTime, rotationAxis.z * Time.deltaTime);
        transform.rotation *= Quaternion.Euler(rotationAxis.x * Time.unscaledDeltaTime, rotationAxis.y * Time.unscaledDeltaTime, rotationAxis.z * Time.unscaledDeltaTime);
    }
}

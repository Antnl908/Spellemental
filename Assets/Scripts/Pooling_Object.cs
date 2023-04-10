using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Pooling_Object : MonoBehaviour
{
    //Made by Daniel.

    public virtual void Initialize(Vector3 position, Quaternion rotation, Vector3 direction, IObjectPool<Pooling_Object> pool)
    {
        Debug.LogError("Initialize not implemented");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTest : MonoBehaviour
{
    [SerializeField] Transform t;
    [SerializeField] float amount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(t == null) { return; }
        transform.position = t.position + t.forward * amount;
    }
}

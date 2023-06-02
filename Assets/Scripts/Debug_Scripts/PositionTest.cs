using UnityEngine;

public class PositionTest : MonoBehaviour
{
    [SerializeField] Transform t;
    [SerializeField] float amount;

    // Update is called once per frame
    void Update()
    {
        if(t == null) { return; }

        transform.position = t.position + t.forward * amount;
    }
}

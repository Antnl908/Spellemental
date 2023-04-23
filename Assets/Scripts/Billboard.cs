using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] bool lookAtCamera = true;
    [SerializeField] bool contstrainYAxis = true;
    Vector3 direction;

    //private void OnWillRenderObject()
    //{
    //    //Direction = Camera.current.transform.position - transform.position;
    //    Direction = Camera.main.transform.position - transform.position;
    //    transform.rotation = Quaternion.LookRotation(Direction, Vector3.up);
    //}
    
    private void Update()
    {
        //Direction = Camera.current.transform.position - transform.position;
        Direction = Camera.main.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(Direction, Vector3.up);
    }

    public Vector3 Direction
    {
        get { return lookAtCamera? -direction : direction; }
        set { direction = value; if (contstrainYAxis) { direction.y = 0f; } }
    }
}

using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] bool lookAtCamera = true;
    [SerializeField] bool contstrainYAxis = true;
    Vector3 direction;

    [SerializeField]
    private bool usesLookAt = false;

    [SerializeField]
    private Vector3 lookAtExtraRotation = Vector3.zero;

    //Works with sprites but doesn't seem to work with some meshes
    //private void OnWillRenderObject()
    //{
    //    //Direction = Camera.current.transform.position - transform.position;
    //    Direction = Camera.main.transform.position - transform.position;
    //    transform.rotation = Quaternion.LookRotation(Direction, Vector3.up);
    //}
    

    /// <summary>
    /// Rotate object towards camera
    /// </summary>
    private void Update()
    {
        if(usesLookAt)
        {
            Quaternion target = Quaternion.Euler(Camera.main.transform.position - transform.position);

            Quaternion offset = Quaternion.Euler(lookAtExtraRotation);

            transform.rotation = target * offset;
        }
        else
        {
            //Direction = Camera.current.transform.position - transform.position;
            Direction = Camera.main.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(Direction, Vector3.up);
        }       
    }

    public Vector3 Direction
    {
        get { return lookAtCamera? -direction : direction; }
        set { direction = value; if (contstrainYAxis) { direction.y = 0f; } }
    }
}

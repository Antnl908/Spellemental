using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player_Look : MonoBehaviour
{
    //Made by Anton L

    //[SerializeField]
    private CinemachineVirtualCamera vcam;
    private Vector2 lookVector;
    private float sense = 0.5f;
    private float Xmin = -75f;
    private float Xmax = 75f;
    private float t = 50f;
    private bool interp = false;
    private bool interpFOV = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Update rotation
        VirtualCamera.transform.rotation = Quaternion.Slerp(VirtualCamera.transform.rotation, Quaternion.Euler(-LookVector.y, LookVector.x, 0f), TAmount);

    }
    void LateUpdate()
    {
        //Update position
        VirtualCamera.transform.position = transform.position;

    }

    public CinemachineVirtualCamera VirtualCamera
    {
        get 
        {
            //Just in case there's a situation where the player virtual camera is destroyed, not assigned already
            //or the Player_Move script calls for the camera before this script(because the position is set in LateUpdate)
            if(vcam == null) 
            {
                var go = new GameObject("vcam");
                VirtualCamera = go.AddComponent<CinemachineVirtualCamera>();
                VirtualCamera.m_Lens.FieldOfView = 75f;
                VirtualCamera.Priority = 100;
            }
            return vcam;
        }
        set { vcam = value; }
    }

    public Vector3 Forward
    {
        get { return Vector3.Cross(VirtualCamera.transform.right, Vector3.up); }
    }
    public Vector3 Right
    {
        get { return Vector3.Cross(VirtualCamera.transform.forward, Vector3.up); }
    }

    //The vector used to set camera rotation, mouse delta is applied to this vector
    public Vector2 LookVector
    {
        get { return lookVector; }
        set { lookVector += value * Sense; lookVector.y = Mathf.Clamp(lookVector.y, Xmin, Xmax); }
    }

    //Sensitivity of mouse input
    public float Sense
    {
        get { return sense; }
        set { sense = value; }
    }
    
    //Amount of interpolation on camera rotation
    public float TAmount
    {
        get { return Interpolation? t * Time.deltaTime : 1f; }
        set { t = value; }
    }

    //Should camera rotation be lerped?
    public bool Interpolation
    {
        get { return interp; }
        set { interp = value; }
    }

    //Set/Get VirtualCamera FOV
    public float FOV
    {
        get { return VirtualCamera.m_Lens.FieldOfView; }
        set { VirtualCamera.m_Lens.FieldOfView = value; }
    }

    //Set if cursor should be hidden and confined to game window, can be set to false to use ui menus
    public bool HideCursor
    {
        set { if (value == true) { Cursor.lockState = CursorLockMode.Confined; Cursor.visible = false; }else { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; } }
    }
}

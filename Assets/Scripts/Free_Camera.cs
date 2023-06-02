using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Free_Camera : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;
    private Vector2 lookInput;
    private Vector2 lookVector;
    private Vector2 inputDirection;
    private Vector3 moveVector;
    float moveMagnitude;

    [SerializeField]
    private float mouseSensitivity = 0.5f;
    [SerializeField]
    private float Xmin = -75f;
    [SerializeField]
    private float Xmax = 75f;
    [SerializeField]
    private float t = 50f;
    private bool interp = false;
    public bool useFreeCam;

    [SerializeField]
    private float speed = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!useFreeCam) { return; }
        VirtualCamera.transform.rotation = Quaternion.Slerp(VirtualCamera.transform.rotation, Quaternion.Euler(-LookVector.y, LookVector.x, 0f), TAmount);

        Vector3 moveVector = speed * Time.deltaTime * (VirtualCamera.transform.forward * InputDirection.y + VirtualCamera.transform.right * InputDirection.x);
        VirtualCamera.transform.position += moveVector;
    }

    public CinemachineVirtualCamera VirtualCamera
    {
        get
        {
            //Just in case there's a situation where the player virtual camera is destroyed, not assigned already
            //or the Player_Move script calls for the camera before this script(because the position is set in LateUpdate)
            if (vcam == null)
            {
                var go = new GameObject("freeVcam");
                VirtualCamera = go.AddComponent<CinemachineVirtualCamera>();
                VirtualCamera.m_Lens.FieldOfView = 75f;
                VirtualCamera.Priority = 100;
            }
            return vcam;
        }
        set { vcam = value; }
    }

    public void SetPriority(int p)
    {
        VirtualCamera.Priority = p;
    }

    public Vector3 Forward
    {
        get { return Vector3.Cross(VirtualCamera.transform.right, Vector3.up); }
    }
    public Vector3 Right
    {
        //get { return Vector3.Cross(VirtualCamera.transform.forward, Vector3.up); }
        get { return Vector3.Cross(Forward, Vector3.up); }
    }

    //The vector used to set camera rotation, mouse delta is applied to this vector
    public Vector2 LookInput
    {
        get { return lookInput; }
        set { lookInput = value; LookVector = value; }
    }
    public Vector2 LookVector
    {
        get { return lookVector; }
        set { lookVector += value * MouseSensitivity; lookVector.y = Mathf.Clamp(lookVector.y, Xmin, Xmax); }
    }
    public Vector2 InputDirection
    {
        get { return inputDirection; }
        set { inputDirection = value; MoveMagnitude = inputDirection.magnitude; }
    }
    public Vector2 MoveVector
    {
        get { return moveVector; }
        set { moveVector = value; }
    }

    //Sensitivity of mouse input
    public float MouseSensitivity
    {
        get { return mouseSensitivity; }
        set { mouseSensitivity = value; }
    }

    //Amount of interpolation on camera rotation
    public float TAmount
    {
        get { return Interpolation ? t * Time.deltaTime : 1f; }
        set { t = value; }
    }
    //public float OTAmount
    //{
    //    get { if (Vector3.Dot(fpsrig.transform.forward, VirtualCamera.transform.forward) > 0.45f) { return Time.deltaTime * 15f; } else { return 1f; } }
    //    //set { t = value; }
    //}

    //Should camera rotation be lerped?
    public bool Interpolation
    {
        get { return interp; }
        set { interp = value; }
    }

    public float MoveMagnitude
    {
        get { return moveMagnitude; }
        set { moveMagnitude = value; }
        //get { return leaningAmount; }
        //set { leaningAmount = value; }
    }

    //Set/Get VirtualCamera FOV
    public float FOV
    {
        get { return VirtualCamera.m_Lens.FieldOfView; }
        set { VirtualCamera.m_Lens.FieldOfView = value; }
    }

    public bool HideCursor
    {
        set { if (value == true) { Cursor.lockState = CursorLockMode.Confined; Cursor.visible = false; } else { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; } }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player_Look : MonoBehaviour
{
    //Made by Anton L

    private CinemachineVirtualCamera vcam;
    private Vector2 lookVector;

    [SerializeField]
    private float mouseSensitivity = 0.5f;
    [SerializeField]
    private float Xmin = -75f;
    [SerializeField]
    private float Xmax = 75f;
    [SerializeField]
    private float t = 50f;

    private float bobbingValue;
    private float bobb;
    [SerializeField]
    private float bobbingSpeed = 1f;
    [SerializeField]
    private float bobbingAmount = 0.03f;
    private bool isBobbing = true;

    private bool interp = false;
    //private bool interpFOV = false; //implement FOV lerp later   

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Update rotation
        VirtualCamera.transform.rotation = Quaternion.Slerp(VirtualCamera.transform.rotation, Quaternion.Euler(-LookVector.y, LookVector.x, 0f), TAmount);

        //Update headbobbing
        bobb += Time.deltaTime * bobbingSpeed;
        BobbingValue = Mathf.Sin(bobb) * bobbingAmount;

    }
    void LateUpdate()
    {
        //Update position
        VirtualCamera.transform.position = transform.position + Vector3.up * BobbingValue;
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
        set { lookVector += value * MouseSensitivity; lookVector.y = Mathf.Clamp(lookVector.y, Xmin, Xmax); }
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

    //Set true/false to enable/disable head bobbing
    public bool UseBobbing
    {
        set { isBobbing = value; }
    }

    float BobbingValue
    {
        get { return isBobbing ? bobbingValue : 0f; }
        set { if (isBobbing) bobbingValue = value; else bobb = 0f; }
    }

    //Frequency of headbobbing
    public float BobbingSpeed
    {
        get { return bobbingSpeed; }
        set { bobbingSpeed = value; }
    }

    //How high/low 
    public float BobbingAmount
    {
        get { return bobbingAmount; }
        set { bobbingAmount = value; }
    }

    //Set if cursor should be hidden and confined to game window, can be set to false to use ui menus
    public bool HideCursor
    {
        set { if (value == true) { Cursor.lockState = CursorLockMode.Confined; Cursor.visible = false; }else { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; } }
    }
}

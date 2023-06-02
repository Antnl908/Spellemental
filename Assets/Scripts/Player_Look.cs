using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player_Look : MonoBehaviour
{
    //Made by Anton L

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

    private float bobbingValue;
    private float bobb;
    [SerializeField]
    private float bobbingSpeed = 1f;
    [SerializeField]
    private float bobbingAmount = 0.03f;
    private bool isBobbing = true;

    private float swayingValue;
    private float sway;
    //[SerializeField]
    //private float swayingSpeed = 1f;
    [SerializeField]
    private float swayingAmount = 0.03f;
    private bool isSwaying = true;
    
    private float leaningValue;
    private float lean;
    //[SerializeField]
    //private float swayingSpeed = 1f;
    //[SerializeField]
    //private float leaningAmount = 0f;
    private bool isLeaning = true;

    private bool interp = false;
    //private bool interpFOV = false; //implement FOV lerp later   

    [SerializeField]
    private GameObject fpsrig; //Turned into Gameobject - 31/3 - 2023 by Daniel Svensson.
    private Vector3 weaponSway;
    private float weaponSwayMultiplier = 0.5f;

    [SerializeField]
    private GameObject manaOrb;

    // Start is called before the first frame update
    void Start()
    {
        fpsrig.transform.parent = VirtualCamera.transform;

        manaOrb.transform.parent = VirtualCamera.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Update rotation
        if(Time.timeScale > 0)
        {
            VirtualCamera.transform.rotation = Quaternion.Slerp(VirtualCamera.transform.rotation, Quaternion.Euler(-LookVector.y, LookVector.x, LeaningValue), TAmount);
            //fpsrig.transform.localRotation = Quaternion.Slerp(fpsrig.transform.localRotation, WeaponSway, 8f * Time.deltaTime);
        }        
        
        //Update headbobbing/leaning
        bobb += Time.deltaTime * BobbingSpeed;
        sway += Time.deltaTime * SwayingSpeed * MoveMagnitude;
        BobbingValue = Mathf.Sin(bobb) * BobbingAmount;
        SwayingValue = Mathf.Sin(sway) * SwayingAmount;
        LeaningValue = Mathf.Lerp(LeaningValue, LeaningAmount, 7f * Time.deltaTime);

    }
    void LateUpdate()
    {
        //Update position
        VirtualCamera.transform.position = transform.position + Vector3.up * BobbingValue + Right * SwayingValue;
        //fpsrig.transform.position = transform.position + -VirtualCamera.transform.up * 0.25f;
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
        get { return Interpolation? t * Time.deltaTime : 1f; }
        set { t = value; }
    }
    public float OTAmount
    {
        get { if (Vector3.Dot(fpsrig.transform.forward, VirtualCamera.transform.forward) > 0.45f) { return Time.deltaTime * 15f; } else { return 1f; } }
        //set { t = value; }
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
    public bool UseSwaying
    {
        set { isSwaying = value; }
    }
    public bool UseLeaning
    {
        set { isLeaning = value; }
    }

    float BobbingValue
    {
        get { return isBobbing ? bobbingValue : 0f; }
        set { if (isBobbing) bobbingValue = value; else bobb = 0f; }
    }
    float SwayingValue
    {
        get { return isSwaying ? swayingValue : 0f; }
        set { if (isSwaying) swayingValue = value; else sway = 0f; }
    }
    float LeaningValue
    {
        get { return isLeaning ? leaningValue : 0f; }
        set { if (isLeaning) leaningValue = value; else lean = 0f; }
    }

    //Frequency of headbobbing
    public float BobbingSpeed
    {
        get { return bobbingSpeed + MoveMagnitude * 15f; }
        set { bobbingSpeed = value; }
    }
    public float SwayingSpeed
    {
        get { return BobbingSpeed * 0.5f; }
    }
    //public float LeaningSpeed
    //{
    //    get { return swayingSpeed; }
    //    set { swayingSpeed = value; }
    //}

    //How high/low 
    public float BobbingAmount
    {
        get { return bobbingAmount + MoveMagnitude * 0.005f; }
        set { bobbingAmount = value; }
    }
    public float SwayingAmount
    {
        get { return swayingAmount + MoveMagnitude * 0.015f; }
        set { swayingAmount = value; }
    }
    public float LeaningAmount
    {
        get { return Vector3.Dot(Forward * InputDirection.y + Right * -InputDirection.x, Right); }
        //get { return leaningAmount; }
        //set { leaningAmount = value; }
    }
    public float MoveMagnitude
    {
        get { return moveMagnitude; }
        set { moveMagnitude = value; }
        //get { return leaningAmount; }
        //set { leaningAmount = value; }
    }

    public Quaternion WeaponSway
    {
        get
        {
            Quaternion xSway = Quaternion.AngleAxis(-LookInput.y * weaponSwayMultiplier, Vector3.right);
            Quaternion ySway = Quaternion.AngleAxis(LookInput.x * weaponSwayMultiplier, Vector3.up);

            Quaternion targetSway = xSway * ySway;

            return targetSway;
        }
    }

    //Set if cursor should be hidden and confined to game window, can be set to false to use ui menus
    public bool HideCursor
    {
        set { if (value == true) { Cursor.lockState = CursorLockMode.Confined; Cursor.visible = false; }else { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; } }
    }
}

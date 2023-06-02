using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player_Move : MonoBehaviour
{
    //Made by Daniel

    private Player_Controls controls;
    
    private Player_Look look;
    private Free_Camera freeCam;
    private Player_Health health;
    private bool useFreeCam;

    [SerializeField]
    private CharacterController characterController;

    [SerializeField]
    private float speed = 10;

    private bool isGrounded = false;

    [SerializeField]
    private float groundCheckRadius = 0.2f;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private LayerMask groundMask;

    [Range(-10f, -0.01f)]
    [SerializeField]
    private float downwardAcceleration = -1;

    [Range(-100f, -1f)]
    [SerializeField]
    private float maxDownwardAcceleration = -9.82f;

    [SerializeField]
    private float jumpHeight = 10;

    [SerializeField]
    private float bounceHeight = 30;

    private float currentGravitation = 0;

    private readonly Collider[] groundCheckColliders = new Collider[30];

    [Header("Movement events")]
    [Space]
    [SerializeField]
    private UnityEvent OnLanding;

    [SerializeField]
    Transform ui;

    // Start is called before the first frame update
    void Start()
    {
        controls = new();

        controls.Player1.Jump.performed += Jump;

        controls.Player1.Enable();

        health = GetComponent<Player_Health>();

        look = GetComponent<Player_Look>();
        look.HideCursor = true;
        freeCam = GetComponent<Free_Camera>();
        controls.Player1.SwitchCamera.performed += SwitchCamera;


        OnLanding ??= new();
    }

    private void OnDisable()
    {
        controls.Player1.Jump.performed -= Jump;
        controls.Player1.SwitchCamera.performed -= SwitchCamera;
    }
    private void OnEnable()
    {

    }

    void SwitchCamera(InputAction.CallbackContext context)
    {
        useFreeCam = !useFreeCam;
        if (useFreeCam) { freeCam.SetPriority(100); look.SetPriority(0); health.useFreeCam = useFreeCam; ui.gameObject.SetActive(false); } else { freeCam.SetPriority(0); look.SetPriority(100); health.useFreeCam = useFreeCam; ui.gameObject.SetActive(true); }
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 direction = controls.Player1.Move.ReadValue<Vector2>();
        Vector2 lookDirection = controls.Player1.Look.ReadValue<Vector2>();
        
        //Anton L 20/2/2023 edit: trying out camera based movement
        Vector3 moveVector = speed * Time.deltaTime * (look.Forward * direction.y + look.Right * -direction.x + 
                                                  Vector3.up * Mathf.Clamp(currentGravitation, maxDownwardAcceleration, bounceHeight));

        freeCam.LookInput = lookDirection;
        freeCam.InputDirection = direction;
        freeCam.MoveVector = moveVector;
        freeCam.useFreeCam = useFreeCam;

        if (useFreeCam) { return; }
        Fall();

        characterController.Move(moveVector);

        if(Time.timeScale > 0)
        {
            look.LookInput = lookDirection;
            look.InputDirection = direction;
            look.MoveVector = moveVector;
        }
        
        look.UseBobbing = isGrounded;
        look.UseSwaying = isGrounded;

        CheckIfGrounded();
    }

    private void Fall()
    {
        currentGravitation += downwardAcceleration * Time.deltaTime;

        if(isGrounded && currentGravitation < 0)
        {
            Land();
        }
    }

    public void Land()
    {
        currentGravitation = -1;
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if(isGrounded && currentGravitation <= 0)
        {
            currentGravitation += jumpHeight;

            isGrounded = false;
        }
    }

    public void Bounce()
    {
        currentGravitation = Mathf.Clamp(currentGravitation + bounceHeight, maxDownwardAcceleration, bounceHeight);

        isGrounded = false;
    }

    private void CheckIfGrounded()
    {
        bool wasGrounded = isGrounded;

        isGrounded = false;

        int colliderCount = Physics.OverlapSphereNonAlloc(groundCheck.position, groundCheckRadius, groundCheckColliders, groundMask, 
                                                                                                     QueryTriggerInteraction.Collide);

        for(int i = 0; i < colliderCount; i++)
        {
            if (groundCheckColliders[i].gameObject != gameObject)
            {
                isGrounded = true;

                if(!wasGrounded && currentGravitation <= 0)
                {
                    OnLanding.Invoke();

                    return;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }
}

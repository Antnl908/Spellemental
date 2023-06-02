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

    // Start is called before the first frame update
    void Start()
    {
        controls = new();

        controls.Player1.Jump.performed += Jump;

        controls.Player1.Enable();

        look = GetComponent<Player_Look>();
        look.HideCursor = true;

        OnLanding ??= new();
    }

    private void OnDisable()
    {
        controls.Player1.Jump.performed -= Jump;
    }

    // Update is called once per frame
    void Update()
    {
        Fall();

        Vector2 direction = controls.Player1.Move.ReadValue<Vector2>();
        Vector2 lookDirection = controls.Player1.Look.ReadValue<Vector2>();
        
        //Anton L 20/2/2023 edit: trying out camera based movement
        Vector3 moveVector = speed * Time.deltaTime * (look.Forward * direction.y + look.Right * -direction.x + 
                                                  Vector3.up * Mathf.Clamp(currentGravitation, maxDownwardAcceleration, bounceHeight));

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

    /// <summary>
    /// Decreases currentGravitation to make the player fall. If the player has landed on the ground, then the Land() method is called.
    /// </summary>
    private void Fall()
    {
        currentGravitation += downwardAcceleration * Time.deltaTime;

        if(isGrounded && currentGravitation < 0)
        {
            Land();
        }
    }

    /// <summary>
    /// Sets currentGravitation to -1 so the player stays grounded when on the ground.
    /// </summary>
    public void Land()
    {
        currentGravitation = -1;
    }

    /// <summary>
    /// If the player is on the ground, then currentGravitation is increased to make the player jump.
    /// </summary>
    /// <param name="context">Is needed to subscribe this method to a button</param>
    private void Jump(InputAction.CallbackContext context)
    {
        if(isGrounded && currentGravitation <= 0)
        {
            currentGravitation += jumpHeight;

            isGrounded = false;
        }
    }

    /// <summary>
    /// Bounces the player high up into the air.
    /// </summary>
    public void Bounce()
    {
        currentGravitation = Mathf.Clamp(currentGravitation + bounceHeight, maxDownwardAcceleration, bounceHeight);

        isGrounded = false;
    }

    /// <summary>
    /// Checks wheter or not the player is grounded by looking for ground colliders.
    /// </summary>
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

    /// <summary>
    /// In the editor draws the sphere where the ground collision is checked for.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }
}

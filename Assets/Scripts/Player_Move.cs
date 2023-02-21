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

    private float currentGravitation = 0;

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

    // Update is called once per frame
    void Update()
    {
        Fall();

        Vector2 direction = controls.Player1.Move.ReadValue<Vector2>();
        Vector2 lookDirection = controls.Player1.Look.ReadValue<Vector2>();

        //Vector3 moveVector = speed * Time.deltaTime * new Vector3(direction.x, gravitation, direction.y);
        
        //Anton L 20/2/2023 edit: trying out camera based movement
        Vector3 moveVector = speed * Time.deltaTime * (look.Forward * direction.y + look.Right * -direction.x + 
                                                               Vector3.up * Mathf.Clamp(currentGravitation, maxDownwardAcceleration, int.MaxValue));

        characterController.Move(moveVector);

        look.LookVector = lookDirection;

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
        if(isGrounded)
        {
            currentGravitation += jumpHeight;

            isGrounded = false;
        }
    }

    private void CheckIfGrounded()
    {
        bool wasGrounded = isGrounded;

        isGrounded = false;

        Collider[] colliders = Physics.OverlapSphere(groundCheck.position, groundCheckRadius, groundMask);

        for(int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    [SerializeField]
    private float gravitation = -9.82f;

    // Start is called before the first frame update
    void Start()
    {
        controls = new();
        controls.Player1.Enable();
        look = GetComponent<Player_Look>();
        look.HideCursor = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = controls.Player1.Move.ReadValue<Vector2>();
        Vector2 lookDirection = controls.Player1.Look.ReadValue<Vector2>();

        //Vector3 moveVector = speed * Time.deltaTime * new Vector3(direction.x, gravitation, direction.y);
        
        //Anton L 20/2/2023 edit: trying out camera based movement
        Vector3 moveVector = (look.Forward * direction.y + look.Right * -direction.x + Vector3.up * gravitation) * speed * Time.deltaTime;

        characterController.Move(moveVector);

        look.LookVector = lookDirection;

        CheckIfGrounded();
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
            }
        }
    }
}

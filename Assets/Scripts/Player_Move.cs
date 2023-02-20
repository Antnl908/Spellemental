using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Move : MonoBehaviour
{
    private Player_Controls controls;

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
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = controls.Player1.Move.ReadValue<Vector2>();

        Vector3 moveVector = speed * Time.deltaTime * new Vector3(direction.x, gravitation, direction.y);

        characterController.Move(moveVector);

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

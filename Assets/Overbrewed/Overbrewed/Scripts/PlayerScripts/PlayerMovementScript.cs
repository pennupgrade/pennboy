using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{

    Animator animator;
    CharacterController controller;

    public Transform playerBody;

    [Header("Movement")]
    public float speed = 3f;
    public float acceleration = 12f;
    public float decelerationFactor = 1f;
    public float gravity = -9.81f;

    private Vector3 velocity;

    private void Start() {
        controller = playerBody.GetComponent<CharacterController>();
        animator = playerBody.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        Walk();
    }

    void Walk()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float maxSpeed = speed, maxAcc = acceleration;

        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);

  
        float len = direction.magnitude;
        bool isMoving = len > 0.1f;

        // Update Animator parameter
        if (animator != null)
        {
            animator.SetBool("isWalking", isMoving);
        }

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // small downward force to keep grounded
        }

        if (isMoving)
        {
            Vector3 moveDir = direction * speed;
            controller.Move(moveDir * Time.deltaTime);

            // Face movement direction
            if (direction != Vector3.zero)
                playerBody.forward = direction;
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }
}

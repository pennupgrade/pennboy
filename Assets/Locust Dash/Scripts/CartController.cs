using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartController : MonoBehaviour
{
    private float forwardSpeed = 2f;     // Constant forward movement speed
    private float turnSpeed = 5f;        // Speed for horizontal movement
    private float leftBound = -8f;       // Left boundary for movement
    private float rightBound = 8f;       // Right boundary for movement
    private float tiltAngle = 15f;       // Angle to tilt the cart
    private float tiltSpeed = 5f;        // Speed at which the cart tilts
    private Vector3 targetPosition;     // Desired position to move towards
    private float horizontalVelocity;   // Tracks the movement speed horizontally

    void Start()
    {
        targetPosition = transform.position; // Initialize targetPosition
    }

    void Update()
    {
        HandleMovement();
        HandleTilting();
        if (gameObject.transform.position.z > 5f) {
            Counter.stage = 2;
        }
    }

    void HandleMovement()
    {
        // Get input for left/right movement
        float moveInput = Input.GetAxis("Horizontal");

        // Calculate the horizontal movement based on input
        horizontalVelocity = moveInput * turnSpeed;

        // Adjust the target position based on input
        targetPosition.x += horizontalVelocity * Time.deltaTime;

        // Clamp the target position to stay within boundaries
        targetPosition.x = Mathf.Clamp(targetPosition.x, leftBound, rightBound);

        // Apply forward movement
        targetPosition.z += forwardSpeed * Time.deltaTime;

        // Smoothly move towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);
    }

    void HandleTilting()
    {
        // If moving, tilt based on the direction
        if (Mathf.Abs(horizontalVelocity) > 0.1f)
        {
            float tiltDirection = horizontalVelocity > 0 ? -tiltAngle : tiltAngle; // Determine tilt direction
            Quaternion targetRotation = Quaternion.Euler(0, 0, tiltDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * tiltSpeed);
        }
        else
        {
            // Return to neutral rotation when not moving
            Quaternion neutralRotation = Quaternion.Euler(0, 0, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, neutralRotation, Time.deltaTime * tiltSpeed);
        }
    }

    void OnCollisionEnter(Collision col) {
        Debug.Log("Hit object");
        if (col.gameObject.tag == "LocustBall") {
            Counter.collision++;
            Debug.Log("Collision detected with ball");
             Destroy(col.gameObject);
             forwardSpeed = 0f;
        }
        if (col.gameObject.tag == "Locust_Coin") {
            Counter.coins++;
            Debug.Log("Collision detected with coin");
             Destroy(col.gameObject);
        }
    }
}

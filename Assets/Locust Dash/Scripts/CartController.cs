using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartController : MonoBehaviour
{
    private float forwardSpeed = 5f;     // Constant forward movement speed
    private float turnSpeed = 5f;        // Speed for horizontal movement
    private float leftBound = -8f;       // Left boundary for movement
    private float rightBound = 8f;       // Right boundary for movement
    private float tiltAngle = 15f;       // Angle to tilt the cart
    private float tiltSpeed = 5f;        // Speed at which the cart tilts
    private Vector3 targetPosition;     // Desired position to move towards
    private float horizontalVelocity;   // Tracks the movement speed horizontally

    private UI uI;

    void Start()
    {
        targetPosition = transform.position;
        uI = FindObjectOfType<UI>();
    }

    public void ResetCart() 
    {
        transform.position = Vector3.zero;
        targetPosition = transform.position; // Initialize targetPosition

        // Find the UI component in the scene
        uI = FindObjectOfType<UI>();

        // Check if uI is assigned correctly
        if (uI == null)
        {
            Debug.LogError("UI component not found in the scene. Make sure a GameObject with the UI script is present.");
        }
    }

    void Update()
    {
        HandleMovement();
        HandleTilting();
        
        // Update the stage based on the cart's position
        if (gameObject.transform.position.z > 25f) 
        {
            Counter.stage = 2;
        }

        // Trigger the won method when the cart reaches the specified point
        if (gameObject.transform.position.z > 50f) 
        {
                uI.won();
            
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

            Quaternion targetRotation = targetRotation = Quaternion.Euler(0, -90, 0);
            targetRotation = Quaternion.Euler(0, 0, tiltDirection) * targetRotation;

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * tiltSpeed);
        }
        else
        {
            // Return to neutral rotation when not moving
            Quaternion neutralRotation = Quaternion.Euler(0, -90, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, neutralRotation, Time.deltaTime * tiltSpeed);
        }
    }

    void OnCollisionEnter(Collision col) 
    {
        Debug.Log("Hit object");
        if (col.gameObject.tag == "LocustBall") 
        {
            Counter.collision++;
            Debug.Log("Collision detected with ball");
            Destroy(col.gameObject);
            forwardSpeed = 0f;  // Stop the cart when it hits a ball
        }
        else if (col.gameObject.tag == "Locust_Coin") 
        {
            Counter.coins++;
            Debug.Log("Collision detected with coin");
            Destroy(col.gameObject);
        }
    }
}

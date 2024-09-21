using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartController : MonoBehaviour
{
    public float speed = 5f;
    public float leftBound = -3f;  // Adjust as needed
    public float rightBound = 3f;  // Adjust as needed
    public float tiltAngle = 15f;   // Angle to tilt the cart

    private float currentPosition;
    private Vector3 targetPosition;

    void Start()
    {
        currentPosition = transform.position.x;  // Initialize position
        targetPosition = transform.position; // Initialize targetPosition
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        targetPosition.x += moveInput * speed * Time.deltaTime;

        // Clamp the target position
        targetPosition.x = Mathf.Clamp(targetPosition.x, leftBound, rightBound);

        // Smoothly move to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);

        // Calculate and apply tilt based on input
        if (moveInput != 0)
        {
            float tiltDirection = moveInput > 0 ? -tiltAngle : tiltAngle; // Determine tilt direction
            Quaternion targetRotation = Quaternion.Euler(0, 0, tiltDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f); // Smooth rotation
        }
        else
        {
            // Return to neutral position when not moving
            Quaternion neutralRotation = Quaternion.Euler(0, 0, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, neutralRotation, Time.deltaTime * 5f);
        }
    }
}

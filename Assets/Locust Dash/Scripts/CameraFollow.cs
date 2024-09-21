using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform cart;       // Reference to the cart's Transform
    public float smoothSpeed = 0.125f;  // Smooth speed for the camera
    public Vector3 offset;       // Offset position of the camera
    public float leftBound = -3f; // Left boundary for the camera
    public float rightBound = 3f; // Right boundary for the camera

    void LateUpdate()
    {
        // Calculate desired position based on the cart's position and offset
        Vector3 desiredPosition = cart.position + offset;

        // Clamp the desired position to prevent going out of bounds
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, leftBound, rightBound);

        // Smoothly interpolate to the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        // Update the camera's position
        transform.position = smoothedPosition;
    }
}
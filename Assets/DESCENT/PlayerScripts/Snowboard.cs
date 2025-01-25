using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowboard : MonoBehaviour
{
    public float speed = 5f;
    public float raycastDistance = 100f;
    public float buffer = 0.05f;
    public float smoothSpeed = 0.125f; // Added for interpolation

    void Update()
    {
        // Handle player movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        // Raycast to adjust the Y position based on terrain
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance))
        {
            // Interpolate the Y position to match the terrain smoothly
            Vector3 targetPosition = transform.position;
            targetPosition.y = hit.point.y + buffer;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        }
    }
}

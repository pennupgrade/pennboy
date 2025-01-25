using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : MonoBehaviour
{
    public Transform pointA; // The starting position
    public Transform pointB; // The ending position
    public float speed = 2f; // Speed of the wall's movement

    private Vector3 targetPosition;
    private bool movingToB = true;

    void Start()
    {
        // Set initial target position to point B
        targetPosition = pointB.position;
    }

    void Update()
    {
        // Move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Check if the wall has reached the target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            // Switch target position between point A and point B
            if (movingToB)
            {
                targetPosition = pointA.position;
                movingToB = false;
            }
            else
            {
                targetPosition = pointB.position;
                movingToB = true;
            }
        }
    }
}

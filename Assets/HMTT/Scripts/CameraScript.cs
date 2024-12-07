using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public MovementScript player;  // Reference to the player's Transform
    public float speed = 100f;  // Speed at which the camera moves

    private void Start()
    {
        player = FindObjectOfType<MovementScript>();
    }

    void Update()
    {
        if (player != null)  // Check if the player is assigned
        {
            // Get the camera's current position
            Vector3 currentPosition = transform.position;

            // Get the player's position (can be adjusted if you only want X and Z axes, for example)
            Vector3 targetPosition = new Vector3(player.transform.position.x + 6, player.transform.position.y + 2, -10);

            // Move towards the player's position at the specified speed
            transform.position = Vector3.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);
        }
    }
}

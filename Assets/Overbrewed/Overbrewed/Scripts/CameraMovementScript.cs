using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    public Transform player; // Assign the Player object
    public Vector3 offset = new Vector3(0, 2, 5); // Camera offset (Height & Depth)
    public float followSpeed = 5f; // Smooth camera follow speed
    public float boundaryX = 5f; 
    public float boundaryZ = 2.5f; // How far the player moves before the camera follows

    private Vector3 targetPosition;

    void Update()
    {
        //if (player == null) return;

        //Vector3 desiredPosition = player.position + offset;
        //Vector3 cameraPosition = transform.position;

        //// Check if the player is outside the boundary before moving the camera
        //float distanceX = player.position.x - transform.position.x;
        //float distanceZ = player.position.z - transform.position.z - 12f;

        //if (Mathf.Abs(distanceX) > boundaryX)
        //{
        //    float direction = Mathf.Sign(distanceX); // -1 (left) or 1 (right)
        //    cameraPosition.x = Mathf.Lerp(cameraPosition.x, cameraPosition.x + direction * boundaryX, Time.deltaTime * followSpeed);
        //}

        //if (Mathf.Abs(distanceZ) > boundaryZ)
        //{
        //    float direction = Mathf.Sign(distanceZ); // -1 (down) or 1 (up)
        //    cameraPosition.z = Mathf.Lerp(cameraPosition.z, cameraPosition.z + direction * boundaryZ, Time.deltaTime * followSpeed);
        //}

        //// Keep camera Y and Z stable while adjusting X position
        //targetPosition = new Vector3(cameraPosition.x, desiredPosition.y, cameraPosition.z);
        //transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);

        transform.LookAt(player.position);
    }
}

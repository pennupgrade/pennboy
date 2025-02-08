using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    Rigidbody rb;
    CapsuleCollider col;
    public Transform playerBody;

    [Header("Movement")]
    public float speed = 3f;
    public float acceleration = 12f, decelerationFactor = 1f; 

    private void Start() {
        rb = playerBody.GetComponent<Rigidbody>();
        col = playerBody.GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update() {
        Walk();
    }
    void Walk()
    {
        float maxSpeed = speed, maxAcc = acceleration;

        float horizonalInput = Input.GetAxis("Horizontal");
        float veritcalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizonalInput, 0, veritcalInput);
        float len = direction.magnitude;

        if (len > 0)
        {
            rb.velocity += direction / len * Time.deltaTime * maxAcc;

            // Clamp velocity to the maximum speed.
            if (rb.velocity.magnitude > maxSpeed)
            {
                rb.velocity = rb.velocity.normalized * speed;
            }
            if (direction != Vector3.zero) {
                playerBody.forward = direction;
            }
        }
        else
        {
            // If no buttons are pressed, decelerate.
            len = rb.velocity.magnitude;
            float decel = acceleration * decelerationFactor * Time.deltaTime;
            if (len < decel) rb.velocity = Vector3.zero;
            else
            {
                rb.velocity -= rb.velocity.normalized * decel;
            }
        }
    }
}

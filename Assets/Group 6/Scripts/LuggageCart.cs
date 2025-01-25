using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LuggageCart : MonoBehaviour, PushablePullableObject
{
    [SerializeField] Game level;
    public float maxSpeed = 15f;
    private float maxSqrSpeed;

    private Rigidbody cartRigidBody;
    // Start is called before the first frame update
    void Start()
    {
        maxSqrSpeed = maxSpeed * maxSpeed;
        cartRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(cartRigidBody.velocity.sqrMagnitude > maxSqrSpeed)
        {
            cartRigidBody.velocity = cartRigidBody.velocity.normalized * maxSpeed;
        }
    }

    void OnCollisionEnter(Collision collision) {
        PropertyDamageCollider col = collision.gameObject.GetComponent<PropertyDamageCollider>();
        if (collision.gameObject.name == "Fall off zone") {
            level.reduceBudget(100000);
        }
        if (col != null) {
            int damage = col.calculateDamage(collision.impulse.magnitude);
            if (damage != 0) {
                level.reduceBudget(damage);
            }
        }
    }

    public void push(Vector3 raycastIntersectionPoint, Vector3 playerVelocity, Vector3 dir) {
        pushPull(raycastIntersectionPoint, playerVelocity, dir, 0.3f, 0.8f);
    }

    public void pull(Vector3 raycastIntersectionPoint, Vector3 playerVelocity, Vector3 dir) {
        pushPull(raycastIntersectionPoint, playerVelocity, -dir, 0.1f, 0.5f);
    }

    private void pushPull(Vector3 raycastIntersectionPoint, Vector3 playerVelocity, Vector3 dir, float amountOnStill, float maxAmount) {
        Vector3 playerVelocityInRespectToDir = Vector3.Dot(playerVelocity, dir) * dir;
        Vector3 velocityChange = ((maxAmount - amountOnStill) * maxSpeed / (Player_Movement.basePlayerSpeed * Player_Movement.speedUp))* playerVelocityInRespectToDir + amountOnStill * maxSpeed * dir;
        cartRigidBody.AddForceAtPosition(velocityChange, raycastIntersectionPoint, ForceMode.VelocityChange);
    }
}

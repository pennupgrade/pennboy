using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Secret_Coin : MonoBehaviour
{
    [SerializeField] Game level;
    [SerializeField] private AudioSource pickUpSound;

    // rotation speed in rpm (rotations per minute)
    private float rotateSpeed = 30;
    private float degreesPerSecond;
    // Start is called before the first frame update
    void Start()
    {
        degreesPerSecond = 6 * rotateSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, degreesPerSecond * Time.deltaTime);
    }

    void OnTriggerEnter(Collider collision) {
        PropertyDamageCollider col = collision.gameObject.GetComponent<PropertyDamageCollider>();
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "Luggage_Cart") {
            pickUpSound.Play(0);
            Debug.Log("Should have played sound.");
            level.reduceBudget(-100);
            //Debug.Log("Attempted to add");
            Destroy(gameObject);
        }
    }
}

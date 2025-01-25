using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform elevatorPlatform; // The elevator object
    public Transform targetPosition;   // The position where the elevator moves up to
    public Transform startPosition;    // The position where the elevator moves down to (starting position)
    [SerializeField] public float elevatorSpeed; // Speed of the elevator movement
    public float delayTime = 3.0f;     // Delay before the elevator moves back down
    private Transform luggageCart;
    private Transform oldParent; 
    
    private bool playerInTrigger = false;  // To track if player is in the trigger area
    private bool isMoving = false;         // To check if the elevator is already moving

    // Detect when the player enters the trigger area
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player" && !isMoving)
        {
            playerInTrigger = true;
            StartCoroutine(MoveElevatorUp());
        }
        if (other.gameObject.name == "Luggage_Cart" && !isMoving)
        {
            luggageCart = other.transform;
            oldParent = luggageCart.parent;
            luggageCart.SetParent(elevatorPlatform);
        }
    }

    // Detect when the player exits the trigger area
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            playerInTrigger = false;
        }
        if (other.gameObject.name == "Luggage_Cart" && !isMoving)
        {
            other.transform.SetParent(oldParent.transform);
        }
    }

    // Coroutine to move the elevator up
    IEnumerator MoveElevatorUp()
    {
        isMoving = true;

        // Move the elevator to the target position
        while (Vector3.Distance(elevatorPlatform.position, targetPosition.position) > 0.5f)
        {
            elevatorPlatform.position = Vector3.MoveTowards(elevatorPlatform.position, targetPosition.position, elevatorSpeed * Time.deltaTime);
            yield return null;
        }

        // Wait for a delay before moving down
        yield return new WaitForSeconds(delayTime);

        // If the player has exited the trigger area, move the elevator back down
        if (!playerInTrigger)
        {
            StartCoroutine(MoveElevatorDown());
        }
        else
        {
            isMoving = false; // Allow the elevator to move again if the player triggers it again
        }
    }

    // Coroutine to move the elevator down
    IEnumerator MoveElevatorDown()
    {
        if (luggageCart != null)
        {
            luggageCart.SetParent(oldParent);
            luggageCart = null; // Reset the luggage cart reference
        }
        isMoving = true;

        // Move the elevator to the start position
        while (Vector3.Distance(elevatorPlatform.position, startPosition.position) > 0.5f)
        {
            elevatorPlatform.position = Vector3.MoveTowards(elevatorPlatform.position, startPosition.position, elevatorSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false; // The elevator is now ready to be triggered again
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

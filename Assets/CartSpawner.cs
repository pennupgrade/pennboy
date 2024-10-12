using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartSpawner : MonoBehaviour
{
    public GameObject cartPrefab; // Reference to the cart prefab
    public Transform[] waypoints;  // Array of waypoints
    private GameObject currentCart; // Reference to the currently spawned cart

    void Start() 
    {
        // Find WaypointManager in scene
        WaypointManager WaypointManager = FindObjectOfType<WaypointManager>();
        if (WaypointManager != null) 
        {
            waypoints = WaypointManager.waypoints; // get waypoints from manager
        }
    }

    // Call this method to spawn the cart at the specified index
    public void SpawnCartAt(int index)
    {
        if (index < 0 || index >= waypoints.Length)
        {
            Debug.LogError("Invalid waypoint index.");
            return;
        }

        // Destroy the current cart if it exists
        if (currentCart != null)
        {
            Destroy(currentCart);
        }

        // Instantiate the cart at the specified waypoint
        currentCart = Instantiate(cartPrefab, waypoints[index].position, waypoints[index].rotation);

        // Optionally, access and initialize any scripts on the cart
        CartController cartController = currentCart.GetComponent<CartController>();
        if (cartController != null)
        {
            // Reset or initialize any parameters if necessary
            cartController.ResetCart(); // Example method to reset the cart state
        }
    }
}

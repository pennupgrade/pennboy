using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public Transform[] waypoints; // Array to hold references to waypoints

    void Start()
    {
        // Get all child transforms (waypoints) under this GameObject
        waypoints = GetComponentsInChildren<Transform>();

        // Optionally, remove the first element (this GameObject itself)
        if (waypoints.Length > 0) 
        {
            Transform[] temp = new Transform[waypoints.Length - 1];
            for (int i = 1; i < waypoints.Length; i++) 
            {
                temp[i - 1] = waypoints[i];
            }
            waypoints = temp;
        }
    }

    // Visualizing Waypoints
    void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        if (waypoints != null) 
        {
            foreach (Transform waypoint in waypoints)
            {
                Gizmos.DrawSphere(waypoint.position, 0.5f); // draws small sphere
            }
        }
    }
}

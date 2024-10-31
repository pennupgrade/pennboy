using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupZone : MonoBehaviour
{
    public bool isPickupZone; 

    void OnTriggerEnter(Collider other)
    {
        if (isPickupZone && !GameManager.Instance.HasPassenger)
        {
            GameManager.Instance.HasPassenger = true;
            Debug.Log("Picked up a passenger!");
            Destroy(gameObject);
        }
        else if (!isPickupZone && GameManager.Instance.HasPassenger)
        {
            GameManager.Instance.HasPassenger = false;
            GameManager.Instance.ScorePoints();
            Debug.Log("Dropped off a passenger!");
            Destroy(gameObject);
        }
    }
}

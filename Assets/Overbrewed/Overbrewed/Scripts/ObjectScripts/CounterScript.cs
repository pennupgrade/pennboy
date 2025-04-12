using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterScript : MonoBehaviour, IInteractable
{
    public GameObject player;
    private OrderScript order;

    private void Start() {
        player = GameObject.FindWithTag("Player");
        order = FindObjectOfType<OrderScript>();
    } 

    public void Interact() {
        if (player != null)
        {
            PlayerItemHold holdItem = player.GetComponent<PlayerItemHold>();
            if (holdItem != null)
            {
                ItemScript item = holdItem.GetItemHeld();
                if (item != null) {
                    if (item.IsServable) {
                        return;
                    }
                }
            }
        }
    }

    public void Serve() {
        return;
    }
}

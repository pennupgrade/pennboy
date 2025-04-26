using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableScript : MonoBehaviour, IInteractable
{   
    public bool is_empty = true;
    public int currentItemID = -1;
    private GameObject placedItem = null;
    public GameObject player;

    private void Start() {
        player = GameObject.FindWithTag("Player");
    } 

    public void Interact() {
        if (player != null)
        {
            PlayerItemHold holdItem = player.GetComponent<PlayerItemHold>();
            if (holdItem != null){
                if (placedItem != null) {
                    
                }
            }
        }
    }
}

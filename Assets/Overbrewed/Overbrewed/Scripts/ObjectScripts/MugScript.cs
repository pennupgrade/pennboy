using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MugScript : MonoBehaviour
{
    GameObject player;
    bool playerIsClose = false;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(playerIsClose)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PickupMug();
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.tag == "Player")
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerIsClose = false;
        }
    }

    void PickupMug()
    {
        this.transform.SetParent(player.transform);
    }
}

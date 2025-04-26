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
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Player found: " + player); // this should not be null

    }

    // Update is called once per frame
    void Update()
    {
        if(playerIsClose)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Pressed E to pick up");
                PickupMug();
                Debug.Log("playerIsClose is: " + playerIsClose);
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
        this.transform.localPosition = new Vector3((float)(transform.localPosition.x + 0.5), (float)(transform.localPosition.y + 1.5), (float)(transform.localPosition.z));
    }
}

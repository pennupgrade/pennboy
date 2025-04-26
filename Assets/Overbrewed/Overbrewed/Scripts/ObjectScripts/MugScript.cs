using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MugScript : MonoBehaviour
{
    GameObject player;
    bool playerIsClose = false;
    public GameObject mugHeld = null;
    bool holdingMug = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Player found: " + player); // this should not be null

    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsClose)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!holdingMug)
                {
                    PickupMug();
                }
                else
                {
                    PutDownMug();
                }

            }
        } 
    }

    public void ChangeMugColor(Color newColor)
    {
        Renderer mugRenderer = GetComponent<Renderer>();
        if (mugRenderer != null)
        {
            mugRenderer.material.color = newColor;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject.tag == "Player")
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
        mugHeld = this.gameObject;
        this.transform.SetParent(player.transform);
        this.transform.localPosition = new Vector3((float)(transform.localPosition.x + 0.5), (float)(transform.localPosition.y + 1.5), (float)(transform.localPosition.z));
        holdingMug = true;
    }

    void PutDownMug()
    {
        mugHeld = null;
        this.transform.SetParent(null);
        holdingMug = false;
    }
}

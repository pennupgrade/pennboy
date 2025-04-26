using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MugScript : MonoBehaviour
{
    public List<int> contents; // index 0 is amount of milk, index 1 is type of milk
    // int 0 is whole milk, int 1 is almond milk, int 2 is oat milk
    // amount of milk is 0 to 3
    GameObject player;
    bool playerIsClose = false;
    public GameObject mugHeld = null;
    bool holdingMug = false;
    public Renderer mugRenderer;
    // Start is called before the first frame update
    private Vector3 originalPosition;

    void Start()
    {
        contents = new List<int> { 0, 0 };
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Log("Player found: " + player); // this should not be null
        originalPosition = this.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!holdingMug && playerIsClose)
            {
                PickupMug();
            }
            else if (holdingMug) 
            {
                PutDownMug();
            }

        }
    }

    public void ChangeMugColor(Color newColor)
    {
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
        //this.transform.position = originalPosition;
    }

    public void addOatToMug()
    {
        Debug.Log(contents.Count);
        if (this != null && contents != null)
        {
            if (contents[1] != 2)
            {
                Debug.Log("milk amount: " + contents[0] + " and type of milk: " + contents[1]);
                contents[1] = 2;
                contents[0] = 1;
            }
            else
            {
                Debug.Log("incremented amount");
                contents[0]++;
            }
        }
        else
        {
            Debug.Log("not holding mug");
        }

    }

    public void addAlmondToMug()
    {
        Debug.Log(contents.Count);
        if (this != null && contents != null)
        {
            if (contents[1] != 1)
            {
                Debug.Log("milk amount: " + contents[0] + " and type of milk: " + contents[1]);
                contents[1] = 1;
                contents[0] = 1;
            }
            else
            {
                Debug.Log("incremented amount");
                contents[0]++;
            }
        }
        else
        {
            Debug.Log("not holding mug");
        }

    }

    public void addWholeToMug()
    {
        Debug.Log(contents.Count);
        if (this != null && contents != null)
        {
            if (contents[1] != 0)
            {
                Debug.Log("milk amount: " + contents[0] + " and type of milk: " + contents[1]);
                contents[1] = 0;
                contents[0] = 1;
            }
            else
            {
                Debug.Log("incremented amount");
                contents[0]++;
            }
        }
        else
        {
            Debug.Log("not holding mug");
        }

    }

}

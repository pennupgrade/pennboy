using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterScript : MonoBehaviour
{
    //This is the script for the obstacle that is equivalent to a trampoline in Sonic. The function
    //of this script is to give the gameobject the ability to propel the player upwards upon contact.

    //declare variables here
    public float launchForce = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        //The start function is called when the object is created, initialize variables here
        launchForce = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //OnTriggerEnter is called whenever two gameobject colliders come into contact with each other and one collider has the isTrigger property
    //The collider attached to the booster should have the isTrigger property checked
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") //This line checks to see that the collided object is the Player
        {
            //The code that controls what happens when the player steps on the booster goes here
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            playerRb.velocity = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z);
            playerRb.AddForce(Vector3.up * launchForce, ForceMode.Impulse); 
        }
    }
    //Once you're finished writing this script, the actual gameobject must be made. Create the piece of geometry
    //you envision this booster to take and drag this script into it. Add a collider and edit it to the correct size.
    //Then, drag the gameobject into the prefabs folder to turn it into a prefab.
}
            //playerRb.velocity(Vector3.up * 0);
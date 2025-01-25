using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCollision : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody playerRB;
    public Death death; 
    public float dyingFloat;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Tree"){
            //print(col.relativeVelocity.magnitude);
            if (col.relativeVelocity.magnitude > dyingFloat) {
                death.deathDelay += 5f;
            }
        }
    }
}

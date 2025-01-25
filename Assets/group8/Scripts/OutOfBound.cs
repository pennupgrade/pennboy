using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Upon entering the trigger, this GameObject will destroy it
    void OnTriggerEnter(Collider other)
    {
        Destroy(other.gameObject);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sandbox : MonoBehaviour
{
    public GameObject ob;
    // Start is called before the first frame update
    void Start()
    {
        ob = Instantiate(ob);
        ob.transform.SetParent(GetComponent<Transform>());
        ob.transform.position = Vector3.forward * -10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

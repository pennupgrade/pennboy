using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhenFarFromTarget : MonoBehaviour
{
    public float distance;
    public Transform target;

    private void Start()
    {
        if (target == null)
        {
            target = GameObject.FindObjectOfType<SnowboardCharlie2>().transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((target.position.z - transform.position.z) > distance)
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFollow : MonoBehaviour
{
    public Transform cart;
    public float smoothSpeed = 0.125f;
    public float leftBound = -3f;
    public float rightBound = 3f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 desiredPosition = cart.position;
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, leftBound, rightBound);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

    }
}

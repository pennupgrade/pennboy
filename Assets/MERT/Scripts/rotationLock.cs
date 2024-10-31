using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationLock : MonoBehaviour
{
    private Quaternion initialLocalRotation;

    void Start()
    {
        initialLocalRotation = transform.localRotation;
    }

    void LateUpdate()
    {
        float zRotation = transform.parent.eulerAngles.z;
        transform.localRotation = Quaternion.Euler(initialLocalRotation.eulerAngles.x, initialLocalRotation.eulerAngles.y, zRotation);
    }
}

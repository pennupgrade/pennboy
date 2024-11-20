using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDebug : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float platformRadius = 2f;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, platformRadius);
    }
    public void SetPlatformRadius(float platformRadius)
    {
        this.platformRadius = platformRadius;
    }
}

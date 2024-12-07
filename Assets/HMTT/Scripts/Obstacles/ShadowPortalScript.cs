using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPortalScript : MonoBehaviour
{
    [SerializeField] ShadowPortalScript partnerPortal;
    private float cd = 1f;
    private float currcd;
    private bool active { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        currcd = cd;
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (active == false)
        {
            currcd -= Time.deltaTime;
            if (currcd < 0)
            {
                active = true;
                currcd = cd;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            partnerPortal.active = false;
            if (other.gameObject.tag == "Player")
            {
                other.transform.position = partnerPortal.transform.position;
            }
        }
    }
}

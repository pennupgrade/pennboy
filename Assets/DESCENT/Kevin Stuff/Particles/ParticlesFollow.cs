using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesFollow : MonoBehaviour
{
    public SnowboardCharlie2 playerMovement;
    public Transform target;

    public bool freezeRotation;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    bool childrenActive;

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.CurrentState == SnowboardCharlie2.PlayerState.Grounded && !childrenActive && playerMovement.rbv.sqrMagnitude > 50f)
        {
            childrenActive = true;
            foreach (Transform child in transform)
            {
                child.gameObject.GetComponent<ParticleSystem>().Play();
            }
        }
        else if (childrenActive)
        {
            childrenActive = false;
            foreach (Transform child in transform)
            {
                child.gameObject.GetComponent<ParticleSystem>().Stop();

            }
        }

        transform.position = target.position;

        if (!freezeRotation)
            transform.forward = Vector3.RotateTowards(transform.forward, target.forward, 10f * Time.deltaTime, 0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPusher : MonoBehaviour
{
    public Collider targetCollider;

    // Start is called before the first frame update
    /* it exists to detect whether it is going to */
    void Start()
    {
        targetCollider = GetComponent<Collider>();
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Bomb")
        {
            Debug.Log("Exiting pushbomb");
            //other.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) // Replace with your trigger condition
        {
            Debug.Log("clicked");
            GrabObjectsInsideCollider();
        }
    }


    /* push bomb off */
    public void pushBombsOff(int frequency=5, float duration=2f, float strength=100)
    {
        /*if you call this for that duration, the bomb inside will be pushed outside with certain force*/
        StartCoroutine(RollBombOff(frequency, duration, strength));
        Debug.Log("bomb has been pushed off");
        //StartCoroutine();
    }

    IEnumerator RollBombOff(int frequency= 5, float duration=2f, float strength = 100)
    /* 
     * velocity / force applied to bomb should vary according to the distance from the edge 
     * duration = how long it should be applied for. 
     * frequency = how often this force is aplied
     */
    {
        for (int i = 0; i < frequency; i++)
        {
            // Move bomb
            GrabObjectsInsideCollider(strength);

            // Wait for 0.5 seconds
            yield return new WaitForSeconds(duration/ frequency);
        }

    }

    void GrabObjectsInsideCollider(float magnitude=10)
    {
        // Get all colliders overlapping the bounds of the target collider
        Collider[] colliders = Physics.OverlapBox(
            targetCollider.bounds.center,
            targetCollider.bounds.extents,
            targetCollider.transform.rotation
        );

        float colliderRadius = targetCollider.bounds.extents.x;
        foreach (Collider col in colliders)
        {
            // Check if the object has the specified tag (if set)
            if (col.tag == "Bomb")
            {
                Vector3 dir = (col.transform.position - transform.position);
                dir.y = 0;
                float distanceMagnitude = (colliderRadius * 1.2f - dir.magnitude) / colliderRadius;
                // adding 1.2f to give additional boost to the bomb velocity for ones at the edge
                dir = dir / dir.magnitude;
                //col.GetComponent<Rigidbody>().AddForce(dir * distanceMagnitude * 100);
                Vector3 velocity = dir * distanceMagnitude * magnitude;
                velocity.y = col.GetComponent<Rigidbody>().velocity.y;
                col.GetComponent<Rigidbody>().velocity = velocity;
                //Debug.Log("sending force");
            }
        }

    }

}

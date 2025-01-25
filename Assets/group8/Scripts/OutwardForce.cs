using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* legacy code; no longer used from v6 main scene */
public class OutwardForce : MonoBehaviour
{
    public Vector3 startScale;   // The starting Scale
    public Vector3 endScale;     // The end Scale
    public float duration = 10f;    // Duration of the movement in seconds
    public bool isMoving = false;  // Track if the object is currently moving

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //// Check if space is pressed and the object is not already moving
        //if (Input.GetKeyDown(KeyCode.Space) && !isMoving)
        //{
        //    pushBombsOff();
        //}
    }

    public void pushBombsOff(float _duration = -1)
    {
        if (_duration == -1) _duration = duration;
        StartCoroutine(GrowOverTime(startScale, endScale, _duration));
    }

    // Coroutine to move the object from startScale to endScale over the given duration
    IEnumerator GrowOverTime(Vector3 start, Vector3 end, float duration)
    {
        //enabled = true;
        isMoving = true; // Mark the object as moving

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Lerp scale from start to end based on elapsed time
            transform.localScale = Vector3.Lerp(start, end, elapsedTime / duration);

            // Increment elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the object reaches the exact end scale after the movement
        transform.localScale = end;

        isMoving = false; // Mark the object as no longer moving
        transform.localScale = start;


    }
}

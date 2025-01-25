using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour
{

    //public Vector3 closePos = new Vector3(0,24,-12);
    //public Vector3 closeRot = new Vector3(30, 0, 0);
    //Vector3 farPos = new Vector3(0, 21, -30);
    //Vector3 farRot = new Vector3(22.5f, 0, 0);
    public Vector3 closePos = new Vector3(0, 0, 0);
    public Vector3 closeRot = new Vector3(30, 0, 0);
    public Vector3 farPos = new Vector3(0, 6, -14);
    public Vector3 farRot = new Vector3(38, 0, 0);
    public Vector3 posAdjustment;
    public Vector3 rotAdjustment;

    // Start is called before the first frame update
    void Start()
    {
        if (posAdjustment == Vector3.zero)
            posAdjustment = farPos - closePos;
        if (rotAdjustment == Vector3.zero)
            rotAdjustment = farRot - closeRot;
    }

    public void ZoomIn(float duration = 1)
    {
        Vector3 oldPos = transform.localPosition;
        Vector3 oldRot = transform.localRotation.eulerAngles;
        Vector3 newPos = closePos;
        Vector3 newRot = closeRot;
        StartCoroutine(MoveOverTime(oldPos, newPos, oldRot, newRot, duration));
    }

    public void ZoomOut(float duration = 1)
    {
        Vector3 oldPos = transform.localPosition;
        Vector3 oldRot = transform.localRotation.eulerAngles;
        Vector3 newPos = farPos;
        Vector3 newRot = farRot;
        StartCoroutine(MoveOverTime(oldPos, newPos, oldRot, newRot, duration));
    }

    IEnumerator MoveOverTime(Vector3 startPos, Vector3 endPos, Vector3 startRot, Vector3 endRot, float duration)
    {

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Lerp position from start to end based on elapsed time
            transform.localPosition = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            Vector3 currentEulerAngle = Vector3.Lerp(startRot, endRot, elapsedTime / duration);
            transform.localRotation = Quaternion.Euler(currentEulerAngle);

            // Increment elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverflowDetector : MonoBehaviour
{
    private bool doDetect;
    private bool overflowingBombDetected;

    // Start is called before the first frame update
    void Start()
    {
        doDetect = true;
        overflowingBombDetected = false;
    }


    public bool BombOverflowDetected()
    {
        return overflowingBombDetected;
    }

    private void OnTriggerExit(Collider other)
    {
        if (doDetect)
        {
            if (other.gameObject.tag == "Bomb")
            {
                overflowingBombDetected = true;
                Debug.Log("bomb leaving", other);
            }
        }
    }
    public void TerminateDetection()
    {
        /* when this function turns on, overflow detector stops detecting until restarted */
        doDetect = false;
        overflowingBombDetected = false;
        Debug.Log("terminate Detection");
    }

    public void InitiateDetection()
    {
        /* when this function turns on, overflow detector start detecting until terminated */
        doDetect = true;
        Debug.Log("initiate Detection");
    }
}

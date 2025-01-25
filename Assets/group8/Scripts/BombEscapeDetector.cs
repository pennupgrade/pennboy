using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEscapeDetector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Bomb")
        {
            other.GetComponent<Bomb>().RollDownTheSideOfTower();
        }
    }
}

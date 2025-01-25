using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePoints : MonoBehaviour
{
    // Start is called before the first frame update
    private GameManager gm;
    private bool hasEntered;
    private GameObject player;
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dist = player.transform.position - this.transform.position;
        if (dist.magnitude <= 10f && !hasEntered) {
            gm.StackTreeRun();
            hasEntered = true;
        }
    }
}

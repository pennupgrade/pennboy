using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempCreateEnemy : MonoBehaviour
{
    public GameObject enemy;
    private float spawnRate = 2;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private int ticker = 0;
    // Update is called once per frame
    void Update()
    {
        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        } else {
            timer = 0;
            GameObject newenemy = Instantiate(enemy, transform.position, transform.rotation);
            newenemy.name = string.Concat("Enemy ", ticker.ToString());
            ticker++;
        }
    }
}

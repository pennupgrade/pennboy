using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReachingPlane : MonoBehaviour
{
    public bool enemyReachedPlane;
    private bool canDetectEnemy;

    // Start is called before the first frame update
    void Start()
    {
        enemyReachedPlane = false;
        canDetectEnemy = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitiateDetection()
    /* sould only run by wave manager at correct phase*/
    {
        canDetectEnemy = true;
    }

    public void TerminateDetection()
    /* should only run by wave manager at correct phase*/
    {
        canDetectEnemy = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canDetectEnemy)
        {
            if (other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponent<BombGame.Enemy>().RushToCenter();
                TriggerGameOver();
                Debug.Log("Enemy Reached Surface: Game Over");
            }
        }
    }

    private void TriggerGameOver()
    {
        enemyReachedPlane = true;
    }
}

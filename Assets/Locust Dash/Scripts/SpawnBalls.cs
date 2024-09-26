using System.Collections;
using UnityEngine;

public class SpawnBalls : MonoBehaviour
{
    public GameObject ballPrefab;  // The ball prefab to spawn
    private GameObject currentBall;  // The reference to the current spawned ball
    public float spawnInterval = 5.0f;  // Time interval between spawns
    private Coroutine spawnCoroutine;
    public Vector3 spawnPosition = new Vector3(0, 10, 0);
    public int deltaX = 10;
    public int deltaZ = 10;

    void Start()
    {
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnAndDestroyBall());
        }
    }

    IEnumerator SpawnAndDestroyBall()
    {
        yield return new WaitForSeconds(spawnInterval);  // Initial delay
        while (true)
        {
            if (currentBall != null)
            {
                Debug.Log("Destroying ball: " + currentBall.name);
                Destroy(currentBall);
            }

            Vector3 newPos = new Vector3(spawnPosition.x + Random.Range(-deltaX, deltaX), 10, spawnPosition.z + Random.Range(0,deltaZ));


            currentBall = Instantiate(ballPrefab, newPos, Quaternion.identity);
            Debug.Log("Spawned new ball: " + currentBall.name + spawnPosition.ToString());
            // Destroy(currentBall ,n );

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
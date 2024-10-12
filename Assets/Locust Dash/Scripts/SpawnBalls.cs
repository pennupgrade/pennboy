using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnBalls : MonoBehaviour
{
    public GameObject ballPrefab;  // The ball prefab to spawn
    private GameObject currentBall;  // The reference to the current spawned ball
    private float spawnInterval = 4.0f;  // Time interval between spawns
    private Coroutine spawnCoroutine;

    public GameObject cart;
    public Vector3 spawnPosition = Vector3.zero;
    public float deltaX = 3f;
    public float deltaZ = 3f;

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
                // Debug.Log("Destroying ball: " + currentBall.name);
                // Destroy(currentBall);
            }
            spawnPosition = cart.transform.position + cart.transform.forward * 5;

            spawnPosition.x += Random.Range(-deltaX / 2, deltaX / 2);
            spawnPosition.z += Random.Range(0, deltaZ);
            spawnPosition.y = 10;


            currentBall = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
            // Debug.Log("Spawned new ball: " + currentBall.name + spawnPosition.ToString());
            // Destroy(currentBall ,n );

            yield return new WaitForSeconds(spawnInterval);
        }
    }
    
}
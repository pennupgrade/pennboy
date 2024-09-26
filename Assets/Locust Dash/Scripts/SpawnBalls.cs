using System.Collections;
using UnityEngine;

public class SpawnBalls : MonoBehaviour
{
    public GameObject ballPrefab;  // The ball prefab to spawn
    private GameObject currentBall;  // The reference to the current spawned ball
    public float spawnInterval = 5.0f;  // Time interval between spawns
    private Coroutine spawnCoroutine;

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

        currentBall = Instantiate(ballPrefab, transform.position, Quaternion.identity);
        Debug.Log("Spawned new ball: " + currentBall.name);

        yield return new WaitForSeconds(spawnInterval);
    }
}
}
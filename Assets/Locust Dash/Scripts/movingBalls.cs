using System.Collections;
using UnityEngine;

public class MovingBalls : MonoBehaviour
{
    public GameObject ballPrefab;  // The ball prefab to spawn
    private GameObject currentBall;  // The reference to the current spawned ball
    private float spawnInterval = 1.0f;  // Time interval between spawns
    private Coroutine spawnCoroutine;

    private float xVel = 0.05f;  // Speed at which the ball moves along the x-axis

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
                Destroy(currentBall);  // Destroy the previous ball if needed
            }

            // Set spawn position in front of the cart
            spawnPosition = cart.transform.position + cart.transform.forward * 5;

            // Add randomness to x and z
            spawnPosition.x += Random.Range(-deltaX / 2, deltaX / 2);
            spawnPosition.z += Random.Range(0, 2 * deltaZ);
            spawnPosition.y = -0.5f;

            // Spawn the ball
            currentBall = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);

            // Start moving the ball
            StartCoroutine(MoveBall(currentBall));

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator MoveBall(GameObject ball)
    {
        while (ball != null)  // Continue moving while the ball exists
        {
            // Move the ball along the x-axis
            Vector3 newPosition = ball.transform.position;
            newPosition.x += xVel;  

            // Update the ball's position
            ball.transform.position = newPosition;

            if (ball.transform.position.x >= 8f || ball.transform.position.x <= -8f)
            {
                xVel = -xVel;
            }

            yield return null;  // Wait for the next frame
        }
    }
}

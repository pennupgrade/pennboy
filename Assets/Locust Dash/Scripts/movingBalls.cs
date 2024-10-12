using System.Collections;
using UnityEngine;

public class MovingBalls : MonoBehaviour
{
    public GameObject ballPrefab;  // The ball prefab to spawn
    private float spawnInterval = 4.0f;  // Time interval between spawns
    private Coroutine spawnCoroutine;

    private float xVel = 0.01f;  // Speed at which each ball moves along the x-axis

    public GameObject cart;
    public Vector3 spawnPosition = Vector3.zero;
    public float deltaX = 3f;
    public float deltaZ = 3f;

    void Start()
    {
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnBalls());
        }
    }

    IEnumerator SpawnBalls()
    {
        yield return new WaitForSeconds(spawnInterval);  // Initial delay

        while (true)
        {
            if (cart.transform.position.z > 7.5f)
            {
                // Set spawn position in front of the cart
                spawnPosition = cart.transform.position + cart.transform.forward * 5;

                // Add randomness to x and z
                spawnPosition.x += Random.Range(-deltaX / 2, deltaX / 2);
                spawnPosition.z += Random.Range(deltaZ / 2, 2 * deltaZ);
                spawnPosition.y = -0.5f;

                // Spawn a new ball
                GameObject newBall = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);

                // Start moving the new ball
                StartCoroutine(MoveBall(newBall));
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator MoveBall(GameObject ball)
    {
        float ballXVel = xVel;  // Each ball gets its own x velocity

        while (ball != null)  // Continue moving while the ball exists
        {
            // Move the ball along the x-axis
            Vector3 newPosition = ball.transform.position;
            newPosition.x += ballXVel;

            // Update the ball's position
            ball.transform.position = newPosition;

            // Reverse direction if it hits the boundary
            if (ball.transform.position.x >= 8f || ball.transform.position.x <= -8f)
            {
                ballXVel = -ballXVel;
            }

            yield return null;  // Wait for the next frame
        }
    }
}

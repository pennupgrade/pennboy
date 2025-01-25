
using System.Collections;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject speedPrefab; // Reference to the super speed prefab
    public GameObject sizePrefab;// Reference to the super size prefab
    public GameObject jumpPrefab;// Reference to the super jump prefab
    private GameObject activePowerUp; //track the currently spawned powerup
    private float spawnDelay = 3.0f;


    private void Start()
    {

        StartCoroutine(SpawnPowerUpWithDelay());
    }


    private IEnumerator SpawnPowerUpWithDelay()
    {

        while (true)
        {
            if (activePowerUp == null) // Only spawn a new power-up if there isn’t one already
            {
                SpawnPrefab();
            }
            yield return new WaitForSeconds(spawnDelay); // Wait before checking to spawn again
        }
    }

    void SpawnPrefab()
    {
        // Generate a random x-position within the screen bounds
        float randomX = Random.Range(GameObject.Find("leftGoal").transform.position.x / 2, GameObject.Find("rightGoal").transform.position.x / 2);
        float randomY = Random.Range(-0.47f, 1.05f);

        // Create the new spawn position with the random x and random y
        Vector3 spawnPosition = new Vector3(randomX, randomY, -7.564f);
        transform.position = spawnPosition;

        
        GameObject[] powerUpPrefabs = { speedPrefab, sizePrefab, jumpPrefab };
        
        // Randomly select one from the array
        GameObject powerUpPrefab = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];

        // Instantiate the chosen power-up prefab at the spawn position
        activePowerUp = Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
    }
}

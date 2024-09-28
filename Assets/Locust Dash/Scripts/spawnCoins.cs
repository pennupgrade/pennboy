using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCoins : MonoBehaviour
{
    
    public GameObject coinPrefab;

    public UI uI;
    public GameObject cart;
    private float spawnInterval = 200.0f;
    public float deltaX = 3f;
    public float deltaZ = 3f;

    Quaternion rotation = Quaternion.Euler(90, 0, 0);

    private Coroutine spawnCoroutine;

    void Start()
    {
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(Spawn());
        }

    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(spawnInterval);

        while (true)
        {
            Debug.Log("Coin spawn");
            Vector3 spawnPosition = cart.transform.position + cart.transform.forward * 5;

            spawnPosition.x += Random.Range(-deltaX / 2, deltaX / 2);
            spawnPosition.z += Random.Range(0, deltaZ);
            spawnPosition.y = -0.5f;

            Instantiate(coinPrefab, spawnPosition, rotation);

            yield return new WaitForSeconds(spawnInterval);
        }
    }

}

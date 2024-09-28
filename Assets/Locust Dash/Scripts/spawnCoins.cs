using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCoins : MonoBehaviour
{
    
    public GameObject coinPrefab;

    public UI uI;
    public GameObject cart;
    private float spawnInterval = 10.0f;
    public int deltaX = 10;
    public int deltaZ = 10;

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

            spawnPosition.x += Random.Range(-deltaX, deltaX);
            spawnPosition.z += Random.Range(-deltaZ, deltaZ);
            spawnPosition.y = -0.5f;

            Instantiate(coinPrefab, spawnPosition, rotation);

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject == cart) {
            Destroy(gameObject);
            uI.updateCounter();
        }
    }
}

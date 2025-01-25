using UnityEngine;
using System.Collections.Generic;

public class SpawnSurface : MonoBehaviour
{
    // Object to spawn
    public GameObject objectToSpawn;

    // Number of objects to spawn
    public int numberOfObjects = 5;

    void Start()
    {
        //Spawn(Random.Range(0, 2), 12, 2);
        Spawn(Random.Range(0, 2), 11, 2);
        Spawn(Random.Range(0, 3), 8, 3);
        Spawn(Random.Range(0, 4), 4, 2);
    }

    void Update()
    {
    }

    public void Spawn(int numberOfObjects, float initialVelocity=-1f, float initalVariation=-1f)
    {
        //Debug.Log(numberOfObjects + " Enemy Spawned");
        Vector3[] spawnPositions = GenerateSpawnPositions(numberOfObjects);
        SpawnObjects(spawnPositions, initialVelocity, initalVariation);
    }

    Vector3[] GenerateSpawnPositions(int numberOfObjects)
    {
        Transform planeTransform = transform;

        // Get the local scale of the plane (size is 10x10 in local space)
        Vector3 localScale = planeTransform.localScale;

        // Get the global size on the x and z axes, ignoring rotation
        float width = localScale.x * 10;  // 10 is the default plane width in Unity
        float length = localScale.z * 10; // 10 is the default plane length in Unity
        width = Mathf.Max(width, length);


        // getting local positions
        float inc = (numberOfObjects + 1);
        float div = width / inc;
        List<Vector3> spawnPositions = new List<Vector3>();

        for (int i = 1; i <= numberOfObjects; i++)
        {
            float xPosition = i * div - width / 2;
            spawnPositions.Add(new Vector3(xPosition, 0, 0));
        }

        return spawnPositions.ToArray(); // Return the valid positions as an array
    }

    void SpawnObjects(Vector3[] spawnPositions, float initialVelocity=-1, float initalVariation=-1)
    {
        // Instantiate each object at the generated positions
        foreach (Vector3 localPosition in spawnPositions)
        {
            Vector3 position = transform.rotation * localPosition + transform.position;
            var enemy = Instantiate(objectToSpawn, position, transform.rotation * Quaternion.Euler(-90,0,0));
            enemy.transform.SetParent(transform);
            

            

            Debug.Log("spawning" + objectToSpawn.name);
            if (initialVelocity != -1)
            {
                enemy.GetComponent<BombGame.Enemy>().initialClimbHeight = initialVelocity;
            }
            if (initalVariation != -1)
            {
                enemy.GetComponent<BombGame.Enemy>().initalVariation = initalVariation;
            }
            //Debug.Log(position);
        }
    }
}

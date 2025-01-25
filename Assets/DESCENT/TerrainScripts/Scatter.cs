using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;
using System;

public class Scatter : MonoBehaviour
{
    public List<ScatterObject> trees;
    public List<GameObject> lRocks;
    public List<GameObject> sRocks;

    private ObjectPool<ScatterObject> treePool;

    [Range(0, 1)]
    public float treeDist = 1f;
    [Range(0, 1)]
    public float lRockDist = 1f;
    [Range(0, 1)]
    public float sRockDist = 1f;

    [Range(0, 1)]
    public float scatterChance = 0.05f;
    public float globalScale = 1f;
    public Vector2 heightRangeT = new Vector2(-10f, 0f);
    public Vector2 heightRangeR = new Vector2(1f, 2f);
    public Vector2 scaleRangeT = new Vector2(0.5f, 1.5f);
    public Vector2 scaleRangeR = new Vector2(0.8f, 1.2f);
    public float probMapStrength = 2f;

    public float treeTilt = -20f;

    public Transform treeParent;

    private void Start()
    {
        treePool = new ObjectPool<ScatterObject>(
            createFunc: () => Instantiate(trees[0], treeParent),
            actionOnGet: obj => obj.gameObject.SetActive(true),
            actionOnRelease: obj => obj.gameObject.SetActive(false),
            actionOnDestroy: obj => Destroy(obj.gameObject),
            collectionCheck: false,
            defaultCapacity: 1000,
            maxSize: 5000
        );
    }

    public void ScatterObjects(MeshData meshData, Transform parentTransform, ref Action onChunkDeleted)
    {
        if (meshData == null)
        {
            return;
        }

        Vector3[] normals = meshData.getNormals();
        Vector3[] vertices = meshData.getVertices();
        float[] probability = meshData.getProbability();

        for (int i = 0; i < normals.Length; i++)
        {
            Vector3 vertex = vertices[i];

            // Normal Map Probability Scatter
            //float chance = scatterChance * Mathf.Pow(Mathf.Abs(Vector3.Dot(Vector3.up, normals[i])), 10);

            // Noise Map Probability Scatter
            float chance = scatterChance * Mathf.Pow(probability[i], probMapStrength);
            if (vertex.z != 0 && UnityEngine.Random.value < chance)
            {
                ScatterTree(vertex, parentTransform, ref onChunkDeleted);
            }
        }
    }

    //private void ScatterClusterAtVertex(Vector3 vertex, Transform parentTransform)
    //{
    //    int clusterSize = Random.Range(3, 6); // Cluster size between 3 and 5
    //    for (int i = 0; i < clusterSize; i++)
    //    {
    //        Vector3 offset = new Vector3(
    //            Random.Range(-5f, 5f), // Random offset in x
    //            0,                    // No offset in y
    //            Random.Range(-5f, 5f) // Random offset in z
    //        );

    //        Vector3 clusterPosition = vertex + offset;
    //        ScatterObjectAtVertex(clusterPosition, parentTransform);
    //    }
    //}

    //private void ScatterObjectAtVertex(Vector3 vertex, Transform parentTransform)
    //{
    //    if (trees.Count == 0 && lRocks.Count == 0 && sRocks.Count == 0) return;

    //    float rand = Random.value * (treeDist + lRockDist + sRockDist);

    //    if (rand < treeDist && trees.Count > 0)
    //    {
    //        ScatterTree(vertex, parentTransform);
    //    } else if (rand < treeDist + lRockDist && lRocks.Count > 0)
    //    {
    //        ScatterRock(vertex, parentTransform, lRocks[Random.Range(0, lRocks.Count)]);
    //    }
    //    else if (rand < treeDist + lRockDist + sRockDist && sRocks.Count > 0)
    //    {
    //        ScatterRock(vertex, parentTransform, sRocks[Random.Range(0, sRocks.Count)]);
    //    }
    //}

    private void ScatterTree(Vector3 vertex, Transform parentTransform, ref Action onChunkDeleted)
    {
        ScatterObject scatTree = treePool.Get();
        GameObject tree = scatTree.gameObject;

        Vector3 worldPosition = parentTransform.TransformPoint(vertex + Vector3.up * UnityEngine.Random.Range(heightRangeT.x, heightRangeT.y));
        tree.transform.position = worldPosition;

        Quaternion worldRotation = parentTransform.rotation * Quaternion.Euler(treeTilt,0f,0f);
        
        tree.transform.rotation = worldRotation;

        float randomScale = UnityEngine.Random.Range(scaleRangeT.x, scaleRangeT.y) * globalScale;
        Vector3 worldScale = parentTransform.localScale * randomScale;
        tree.transform.localScale = worldScale;

        scatTree.Init(KillObject, ref onChunkDeleted);
    }

    //private void ScatterRock(Vector3 vertex, Transform parentTransform, GameObject rock)
    //{
    //    GameObject instance = Instantiate(rock, vertex, Quaternion.identity);

    //    instance.transform.parent = parentTransform;

    //    // Ensure the instance inherits the parent's transformations
    //    instance.transform.localPosition = vertex + Vector3.up * Random.Range(heightRangeR.x, heightRangeR.y);
    //    instance.transform.localRotation = Quaternion.Euler(
    //        Random.Range(0f, 360f), // Random rotation around x-axis
    //        Random.Range(0f, 360f), // Random rotation around y-axis
    //        Random.Range(0f, 360f)  // Random rotation around z-axis
    //    );

    //    float randomScale = Random.Range(scaleRangeR.x, scaleRangeR.y) * globalScale;
    //    instance.transform.localScale = Vector3.one * 20 * randomScale;
    //}

    private void KillObject(ScatterObject obj)
    {
        treePool.Release(obj);
    }
}

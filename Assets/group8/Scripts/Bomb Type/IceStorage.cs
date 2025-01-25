
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class IceStorage : MonoBehaviour
{

    public static IceStorage Instance { get; private set; }

    public GameObject targetObject;
    public GameObject iceVfx;
    public float radius = 1.5f;
    public float randomness = 0.2f; // Amount to randomize each vertex position
    public Material iceMaterial;

    void Awake()
    {
        // Check if there is already an instance of this class
        if (Instance == null)
        {
            Instance = this;   // Set this instance as the singleton instance
            DontDestroyOnLoad(gameObject); // Optional: keep it between scenes
        }
        else
        {
            Destroy(gameObject); // Destroy this object if an instance already exists
        }

    }

    public void EffectIce(GameObject enemy)
    {
        Debug.Log("Effect Ice triggered");
        GenerateIceIcosahedron(enemy.transform);
        GenerateVRX(enemy.transform);
        Debug.Log("Effect Ice ended");

        // damage on Enemies
        enemy.GetComponent<BombGame.Enemy>().iceFreeze(3);
        enemy.GetComponent<BombGame.Enemy>().TakeDamage(1);
    }

    void GenerateIceIcosahedron(Transform target)
    {
        GameObject iceEncasingObject = new GameObject("Ice Encasing");
        iceEncasingObject.transform.SetParent(target);
        MeshFilter meshFilter = iceEncasingObject.AddComponent<MeshFilter>();
        Mesh mesh = new Mesh();

        // Golden ratio to create icosahedron vertices
        float t = (1 + Mathf.Sqrt(5)) / 2;

        // Basic vertices of an icosahedron
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(-1, t, 0),
            new Vector3(1, t, 0),
            new Vector3(-1, -t, 0),
            new Vector3(1, -t, 0),
            new Vector3(0, -1, t),
            new Vector3(0, 1, t),
            new Vector3(0, -1, -t),
            new Vector3(0, 1, -t),
            new Vector3(t, 0, -1),
            new Vector3(t, 0, 1),
            new Vector3(-t, 0, -1),
            new Vector3(-t, 0, 1)
        };

        // Normalize vertices to form a unit sphere and scale to radius
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = vertices[i].normalized * radius;
            vertices[i] += Random.insideUnitSphere * randomness; // Randomize each vertex slightly
            vertices[i] += target.position; // Position around target object
        }

        // Icosahedron faces (triangles)
        int[] triangles = new int[]
        {
            0, 11, 5, 0, 5, 1, 0, 1, 7, 0, 7, 10, 0, 10, 11,
            1, 5, 9, 5, 11, 4, 11, 10, 2, 10, 7, 6, 7, 1, 8,
            3, 9, 4, 3, 4, 2, 3, 2, 6, 3, 6, 8, 3, 8, 9,
            4, 9, 5, 2, 4, 11, 6, 2, 10, 8, 6, 7, 9, 8, 1
        };

        // Assign vertices and triangles to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // Set mesh to the mesh filter
        meshFilter.mesh = mesh;

        // Set material
        if (iceMaterial != null)
        {
            iceEncasingObject.AddComponent<MeshRenderer>().material = iceMaterial;
        }
    }

    void GenerateVRX(Transform target)
    {
        GameObject vfx = Instantiate(iceVfx);
        vfx.transform.SetParent(target);
        vfx.transform.localPosition = Vector3.zero;
    }
}

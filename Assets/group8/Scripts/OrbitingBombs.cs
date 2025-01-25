using UnityEngine;
using System.Collections;

public class OrbitingBombs : MonoBehaviour
{
    public GameObject objectPrefab; // Prefab of the object to instantiate
    public float[] angles;         // Angles to orbit
    public float[] speeds;         // Speeds for each arc segment
    public float[] bombSizes = new float[] { 1f, 1.5f, 2f, 2.5f, 3f, 3.3f, 3.6f, 3.9f, 4.3f };
    private float stdSize = 0.3f;
    private float radius;          // Radius of the orbit (calculated from the cylinder's scale)
    private bool isOrbiting = false;  // To track if the objects are currently orbiting

    // Array to store references to instantiated objects
    private GameObject[] orbitingObjects;
    private int[] orbitAngle;

    void Start()
    {
        if (angles.Length != speeds.Length)
        {
            Debug.LogError("The number of angles and speeds must be the same.");
            return;
        }

        // Calculate the radius using the cylinder's local scale
        radius = 0.5f;

        // Initialize the array to store orbiting objects
        orbitingObjects = new GameObject[angles.Length];
        orbitAngle = new int[angles.Length];

    }

    public void SetUpPallette(int[] levels)
    {
        /* please load levels with length 3 */
        for (int i = 0; i < levels.Length; i++)
        {
            orbitingObjects[i] = CreateObjectAtAngle(levels[i], angles[levels.Length - i]);
            orbitAngle[i] = levels.Length - i;
        }
            
    }

    GameObject CreateObjectAtAngle(int newBombLevel, float angle)
    {

        // Instantiate the object only if it doesn't already exist
        // Instantiate the child object in the world space (without a parent initially)
        GameObject child = Instantiate(objectPrefab, Vector3.zero, Quaternion.identity, transform);
        FakeBomb bombProperty = child.GetComponent<FakeBomb>();

        bombProperty.level = newBombLevel;
        child.transform.localScale *= bombSizes[0] * stdSize;

        SetObjectPosition(child, angle); // Set initial position
        return child;
    }

    // Start orbiting when the function is called
    public void StartOrbit(int newBombLevel=4)
    {
        if (isOrbiting) return; // Prevent multiple orbit initiations
        isOrbiting = true;

        // Start the coroutine for each object
        bool alreadyCreatedObject = false;
        for (int i = 0; i < angles.Length; i++)
        {
            if (orbitingObjects[i] == null)
            {
                if (alreadyCreatedObject) continue;

                orbitingObjects[i] = CreateObjectAtAngle(newBombLevel, angles[0]);
                orbitAngle[i] = 0;
                //Debug.Log("Creating object at " + angles[orbitAngle[i]]);
                alreadyCreatedObject = true;
            }

            // Handle the last object (destroy it instead of moving it to the first angle)
            if (orbitAngle[i] == angles.Length-1)
            {
                //Debug.Log("Destroying object at " + angles[orbitAngle[i]]);
                Destroy(orbitingObjects[i]);
                orbitingObjects[i] = null;
                orbitAngle[i] = -1;
            }
            else
            {
                StartCoroutine(MoveObject(i));
            }
        }
    }

    // Coroutine to move each object
    private IEnumerator MoveObject(int index)
    {
        GameObject obj = orbitingObjects[index];
        int nextAngleIndex = (index + 1) % angles.Length;  // Get the next angle in a cyclic manner
        float startAngle = angles[orbitAngle[index]];
        orbitAngle[index]++;
        float endAngle = angles[orbitAngle[index]];
        //Debug.Log("Moving object " + index + " from " + startAngle + " to " + endAngle);

        // The time to move is the angle difference divided by the speed
        float timeToMove = Mathf.Abs(endAngle - startAngle) / speeds[index];
        float elapsedTime = 0;

        // If the angle difference is negative (i.e., going from 350 to 10), we want to move counterclockwise
        if (endAngle < startAngle)
        {
            endAngle += 360;  // Ensure we are moving in a counterclockwise direction
        }

        // Move the object from its current position to the next angle
        while (elapsedTime < timeToMove)
        {
            float t = elapsedTime / timeToMove;
            float currentAngle = Mathf.Lerp(startAngle, endAngle, t);
            SetObjectPosition(obj, currentAngle);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the object reaches the target angle
        SetObjectPosition(obj, endAngle);

        // Mark orbiting as complete if it's the last object
        //if (index == angles.Length - 2)
        if (orbitAngle[index] == 1) isOrbiting = false;
    }

    // Set the object's position based on the angle
    void SetObjectPosition(GameObject obj, float angle)
    {
        Vector3 localPosition = new Vector3(
            radius * Mathf.Cos(angle * Mathf.Deg2Rad),
            0, // Assume we are orbiting in the horizontal plane
            radius * Mathf.Sin(angle * Mathf.Deg2Rad)
        );

        obj.transform.localPosition = localPosition;
    }

    //private void Update()
    //{
    //    if (Input.GetKey("space"))
    //    {
    //        StartOrbit();
    //    }
    //}
}

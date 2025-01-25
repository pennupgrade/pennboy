using UnityEngine;

public class Forest : MonoBehaviour
{
    public Transform player; // Assign the player object in the inspector
    public float viewAngle = 90f; // Angle of visibility in degrees
    public float renderDistance = 50f; // Maximum distance for rendering trees

    private Transform[] treeTransforms;
    private Vector3[] originalPos;
    private Vector3[] newPos;

    void Start()
    {
        // Get all child transforms (trees)
        int childCount = transform.childCount;
        treeTransforms = new Transform[childCount];
        originalPos = new Vector3[childCount];
        newPos = new Vector3[childCount];
        for (int i = 0; i < childCount; i++)
        {
            treeTransforms[i] = transform.GetChild(i);
            originalPos[i] = treeTransforms[i].position;
            Vector3 xzVector = originalPos[i];
            xzVector.y = 0;
            newPos[i] = treeTransforms[i].position + xzVector.normalized * 5;
            //newPos[i] = treeTransforms[i].position + Vector3.down * 20;
        }
    }

    void Update()
    {
        for(int i = 0; i < treeTransforms.Length;i++)
        { 
            Transform tree = treeTransforms[i];
            float angle = IsObjectInFront(player, tree.position);
            float alpha = 1 - (angle / 180);
            float threshold = 0.3f;
            //if (alpha > threshold) tree.position = originalPos[i];
            //else
            //{
            //    tree.position = originalPos[i] * (alpha / threshold) + newPos[i] * (1-(alpha / threshold));
            //}

            //if (alpha > threshold)
            //{
            //    MeshRenderer meshRenderer = tree.GetComponent<MeshRenderer>();
            //    foreach (Material mat in meshRenderer.materials)
            //    {
            //        Color color = mat.color;
            //        color.a = 1f;
            //        mat.color = color;
            //    }
            //}
            //else
            //{
            //    MeshRenderer meshRenderer = tree.GetComponent<MeshRenderer>();
            //    foreach (Material mat in meshRenderer.materials)
            //    {
            //        Color color = mat.color;
            //        color.a = 0.5f; //  1f * (alpha / threshold) + 0.5f * (1 - (alpha / threshold));
            //        Debug.Log(color.a);
            //        mat.color = color;
            //    }
            //}
                


            //tree.gameObject.SetActive(IsObjectInFront(player, tree.position));

        }
    }

    private float IsObjectInFront(Transform player, Vector3 objectPosition)
    {

        Vector3 toObject = objectPosition - player.position; // Direction from player to the object
        Vector3 playerDirection = player.forward;
        Vector3 playerForwardXZ = new Vector3(playerDirection.x, 0, playerDirection.z).normalized;
        Vector3 toObjectXZ = new Vector3(toObject.x, 0, toObject.z).normalized;
        float angle = Vector3.Angle(playerForwardXZ, toObjectXZ);
        // If dot product is less than 0, the object is behind
        return angle;
    }

    public float fadeSpeed = 2f;

}

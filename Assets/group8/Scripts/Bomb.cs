using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    /* 
     * bomb of same level can combine
     * Should create new bomb in its existant place
     * */

    // Constants
    public GameObject basicBomb;
    public GameObject fleshBomb;
    public GameObject magmaBomb;
    public GameObject FireBombVFX;

    // changeable variables
    public int level = 0;
    public float newBombPositionParameter = 0.5f; // must be between 0 and 1

    // private variable
    private float[]  bombSizes = new float[] { 1f, 1.5f, 2f, 2.5f, 3f, 3.3f, 3.6f, 3.9f, 4.3f };
    private int[] level2Score = new int[] { 3, 5, 10, 20, 30, 50, 70, 90, 120, 150};
    private System.DateTime born = System.DateTime.Now;
    public int parentsLevel = -1; // -1 means had no parent bomb
    private bool fromCombined = false;
    ScoreManager scoreManager;

    // Reference to the new material to apply
    public Material[] level2Color;
    public Material[] magmaMaterial;
    private int n_material;

    private bool isRollingDownSideOfTower = false;


    // Start is called before the first frame update
    void Start()
    {
        if (parentsLevel != -1)
            level = parentsLevel + 1;

        // set color
        n_material = level2Color.Length;
        SetMaterial(level2Color[Mathf.Min(level, n_material)]);

        // set size
        if (fromCombined) // parentsLevel always larger than -1
        {
            float startSize = bombSizes[parentsLevel];
            float endSize = bombSizes[level];
            StartCoroutine(ScaleOverTime(startSize, endSize, 0.25f));
        }
        else
            transform.localScale = Vector3.one * bombSizes[level];

        //Debug.Log("my level" + level);
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        // general collision detection
        ContactPoint contact = collision.contacts[0];
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 position = contact.point;

        //Debug.Log("something collided" + position);

        // Bomb collision detection
        if (collision.gameObject.tag == "Bomb")
        {
            Bomb collidedBomb = collision.gameObject.GetComponent<Bomb>();

            //Debug.Log("bombLevels " + level + " " + collidedBomb.level);

            // bomb of same level can combine
            if (level == collidedBomb.level)
            {
                // destroy both bomb and create new bomb in its place
                

                // only procreate when you are born first
                if (born < collidedBomb.born)
                {
                    // determine new position based on which one is born first
                    Vector3 bornFirstPosition = born < collidedBomb.born ? transform.localPosition : collidedBomb.transform.localPosition;
                    Vector3 bornSecondPosition = born > collidedBomb.born ? transform.localPosition : collidedBomb.transform.localPosition;
                    float alpha = newBombPositionParameter;
                    Vector3 newPosition = alpha * bornFirstPosition + (1 - alpha) * bornSecondPosition;

                    // create bomb and set its parentslevel
                    GameObject newBomb;
                    if (level == 5) newBomb = Instantiate(fleshBomb, newPosition, Quaternion.identity);
                    else if (level == 2) newBomb = Instantiate(magmaBomb, newPosition, Quaternion.identity);
                    else newBomb = Instantiate(basicBomb, newPosition, Quaternion.identity);

                    Bomb bombProperty = newBomb.GetComponent<Bomb>();
                    bombProperty.parentsLevel = level;
                    bombProperty.fromCombined = true;

                    // add score
                    int score = level2Score[level];
                    if (scoreManager != null)
                    {
                        scoreManager.addScore(score);
                        scoreManager.reportBombLevel(level+1);
                    }
                }
                Destroy(gameObject);
                
            }

        }
        if (collision.gameObject.tag == "Enemy")
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, bombSizes[level] + 1);
            if (level == 1 || level == 2)
            {
                GameObject explosionVFX = Instantiate(FireBombVFX);
                explosionVFX.transform.localScale *= (bombSizes[level] + 1) / 2.5f;
                explosionVFX.transform.position = transform.position;
            }
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.gameObject.tag == "Enemy")
                {

                    Debug.Log("enemy touched with level" + level);
                    hitCollider.GetComponent<BombGame.Enemy>().TouchedByBomb(level);
                }
                Destroy(gameObject);
            }
        }
    }


    private IEnumerator ScaleOverTime(float startScale, float endScale, float duration)
    {
        float timeElapsed = 0f;
        Vector3 initialScale = Vector3.one * startScale;
        Vector3 targetScale = Vector3.one * endScale;

        while (timeElapsed < duration)
        {
            // Lerp between initialScale and targetScale over time
            transform.localScale = Vector3.Lerp(initialScale, targetScale, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null; // Wait until next frame
        }

        // Ensure the final scale is exactly the target scale
        transform.localScale = targetScale;
    }


    // Function to change the object's material
    public void SetMaterial(Material material)
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            if (renderer.materials.Length == 1)
                renderer.material = material;
            else if (renderer.materials.Length == 2)
                renderer.materials = magmaMaterial;
        }
        else
        {
            Debug.LogWarning("Renderer component not found on this GameObject.");
        }
    }

    public void RollDownTheSideOfTower()
    {
        // needs to run exactly once
        if (!isRollingDownSideOfTower)
        {
            isRollingDownSideOfTower = true;
            StartCoroutine(DoRollDownSideOfTower());
            Debug.Log("bomb has been pushed off");
        }
        //StartCoroutine();
    }

    IEnumerator DoRollDownSideOfTower(float frequency=5, float duration = 2, float strength = 100)
    {

        for (int i = 0; i < frequency; i++)
        {
            Vector3 dir = Vector3.zero - transform.position;
            dir.y = 0;
            dir /= dir.magnitude;
            float magnitude = strength * (1 - (i / frequency));
            // increasing magnitude over time

            // Move bomb
            GetComponent<Rigidbody>().AddForce(dir * magnitude);

            // Wait for 0.5 seconds
            yield return new WaitForSeconds(duration / frequency);
        }

        
        
    }


}

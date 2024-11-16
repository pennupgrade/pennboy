using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Throw : MonoBehaviour
{

    public GameObject thrownObject;

    public LineRenderer lineRenderer;
    [Min(3)] // ensuring that value is at least 3
    private int lineSegments = 30;
    [SerializeField, Min(1)] // makes private variable visible for testing, ensuring that value is at least 1
    private float timeOfTheFlight = 2;

    public float trajectoryDepthLimit = 0f; //line isn't drawn below this point

    public float defaultAngle = 30;
    public float defaultMagnitude = 7;
    public float angleControlScale = 1;
    public float magControlScale = 20;
    public float maxAngle = 30.70f;
    public float minAngle = 29.95f;
    public float maxMag = 10.70f;
    public float minMag = 6.75f;

    public float rateBySeconds = 1; // 2 means 2 bomb per seconds
    int bombCreationLevel = 0;
    public int overrideTestBomb = -1; // anything other than -1 will allow you to test with different level bomb

    [SerializeField]
    private float angle = 30;
    [SerializeField]
    private float magnitude = 10;


    [SerializeField]
    private bool handActive; // turn this on to make the hand be able to throw bomb

    // mouse drag information
    private Vector3 mouseStartPosition;

    private float lastThrownTime;
    private ScoreManager scoreManager;


    // Start is called before the first frame update
    void Start()
    {
        mouseStartPosition = Input.mousePosition;
        handActive = true;
        lastThrownTime = Time.time;
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    public void activateHand(bool activate)
    {
        handActive = activate;
        lineRenderer.enabled = activate;
    }

    // Update is called once per frame
    void Update()
    {
        if (handActive)
        {

            // get mouse position
            float normMousePos = -0.5f + Input.mousePosition.y / Screen.height * 2f;
            normMousePos = Mathf.Max(normMousePos, 0);
            normMousePos = Mathf.Min(normMousePos, 1);
            angle = minAngle + (maxAngle - minAngle) * normMousePos;
            magnitude = minMag + (maxMag - minMag) * normMousePos;

            Vector3 startPoint = transform.position;
            Vector3 startVelocity = GetStartVelocity(angle, magnitude);

            // Draw trajectory on game board
            ShowTrajectoryLine(startPoint, startVelocity);

            // Detect mouse release (end of the drag)
            if (Input.GetMouseButtonDown(0))
            {
                // Calculate and print the extent of the drag
                Vector3 dragExtent = Input.mousePosition - mouseStartPosition;

                float currentTime = Time.time;
                if (currentTime > lastThrownTime + 1 / rateBySeconds)
                {
                    CreateBomb(startPoint, startVelocity);
                    lastThrownTime = currentTime;

                    //add score
                    //int score = 5;
                    //if (scoreManager != null) scoreManager.addScore(score);
                }
            }
        }

    }

    Vector3 GetStartVelocity(float angle, float magnitude)
    {
        Vector3 forward = transform.forward;
        Vector3 forwardOnXZPlane = new Vector3(forward.x, 0, forward.z).normalized;

        float yComponent = Mathf.Sin(Mathf.Deg2Rad * angle);
        float horizontalMagnitude = Mathf.Cos(Mathf.Deg2Rad * angle);

        Vector3 velocity = forwardOnXZPlane * horizontalMagnitude;
        velocity.y = yComponent;
        velocity *= magnitude;
        return velocity;
    }

    void CreateBomb(Vector3 startPoint, Vector3 startVelocity)
    {
        var bomb = Instantiate(thrownObject, startPoint, Quaternion.Euler(0, 0, 30));
        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        rb.velocity = startVelocity;
        int level = Random.Range(0, bombCreationLevel+1);
        level = (overrideTestBomb == -1) ? level : overrideTestBomb;
        bomb.GetComponent<Bomb>().level = level;
        bomb.GetComponent<Bomb>().parentsLevel = level - 1;


    }

    public void setBombCreationLevel(int level)
    /* this is the level of bomb you can create up to, so if 2, then 0,1,2 is all possible*/
    {
        Debug.Log("Bomb Creation Level set as " + level.ToString());
        bombCreationLevel = level;
    }

    public void ShowTrajectoryLine(Vector3 startpoint, Vector3 startVelocity)
    {
        /*https://www.youtube.com/watch?v=U3hovyIWBLk*/

        //The more points we add the smoother the line will be
        float timeStep = timeOfTheFlight / lineSegments;

        Vector3[] lineRendererPoints = CalculateTrajectoryLine(startpoint, startVelocity, timeStep);

        //for (int i = 0; i < lineRendererPoints.Length; i++)
        //    Debug.Log(lineRendererPoints[i].ToString());

        lineRenderer.positionCount = lineRendererPoints.Length;
        lineRenderer.SetPositions(lineRendererPoints);
    }

    private Vector3[] CalculateTrajectoryLine(Vector3 startpoint, Vector3 startVelocity, float timestep)
    {
        List<Vector3> lineRendererPoints = new List<Vector3>();
        lineRendererPoints.Add(startpoint);

        for (int i = 1; i < lineSegments; i++)
        {
            float timeOffset = timestep * i;

            Vector3 progressBeforeGravity = startVelocity * timeOffset;
            Vector3 gravityOffset = Vector3.up * -0.5f * Physics.gravity.y * timeOffset * timeOffset;
            Vector3 newPosition = startpoint + progressBeforeGravity - gravityOffset;
            if (newPosition.y < trajectoryDepthLimit) break;
            else lineRendererPoints.Add(newPosition);

        }
        return lineRendererPoints.ToArray();

    }
}

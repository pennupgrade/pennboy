using System.Collections;
using UnityEngine;
using TMPro;

public class ballScript : MonoBehaviour
{
    public GameObject ballPrefab; // Reference to the ball prefab
    public Camera mainCamera; // Reference to the main camera
    public float fixedYPosition = 4.78f; // The constant y-position you want the ball to spawn at
    public float spawnDelay = 1f;
    public TextMeshProUGUI scoreText;
    private float leftBoundary;
    private float rightBoundary;
    public int player1score;
    public int player2score;

    private void Start()
    {
        // Calculate the world-space boundaries of the camera's view based on orthographic size
        float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;

        // Set the x-boundaries of the camera view
        leftBoundary = mainCamera.transform.position.x - cameraHalfWidth;
        rightBoundary = mainCamera.transform.position.x + cameraHalfWidth;
    }

    private void Update()
    {
        if (transform.position.x < leftBoundary || transform.position.x > rightBoundary)
        {

            GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
            GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);

            SpawnBall();
            
        }

        PlayerPrefs.SetInt("Player 1 Score", player1score);
        PlayerPrefs.SetInt("Player 2 Score", player2score);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PennSoccerGoalL")
        {
            player2score++;
            UpdateScoreText();
        }
        if(other.gameObject.tag == "PennSoccerGoalR")
        {
            player1score++;
            UpdateScoreText();
        }
    }

    void SpawnBall()
    {
        // Get the world position of the camera's edges (orthographic camera assumed)
       // float cameraHeight = 2f * mainCamera.orthographicSize;
        //float cameraWidth = cameraHeight * mainCamera.aspect;
       
        // Generate a random x-position within the screen bounds
        float randomX = Random.Range(GameObject.Find("leftGoal").transform.position.x / 2, GameObject.Find("rightGoal").transform.position.x / 2);


        // Create the new spawn position with the random x and fixed y
        Vector3 spawnPosition = new Vector3(randomX, fixedYPosition, -7.564f);
        transform.position = spawnPosition;
    
      
    }

    void UpdateScoreText()
    {
        scoreText.text = player1score + "   " + player2score;
    }
}

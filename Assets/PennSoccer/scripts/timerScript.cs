using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float timeLeft = 120f;
    public bool gameEnd = false;
    public GameObject ball;


    void Start()
    {
         ball = GameObject.FindGameObjectWithTag("PennSoccerBall");
    }
    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (timeLeft <= 0)
        {
            gameEnd = true;
            SceneManager.LoadScene(sceneName: "WinScene");
        }

    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.PackageManager;
public class UI : MonoBehaviour
{
     public TMP_Text scoreText;

     public GameObject winningCanvas;

     public GameObject play;
     public TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        coinCounter();
    }

    // Update is called once per frame
    void Update()
    {
        coinCounter();
        stageText();
    }

     public void coinCounter() {
        scoreText.text = "Coins: " + Counter.coins.ToString();
    }

    public void stageText() {
        text.text = "Stage: " + Counter.stage.ToString();
    }

    public void won() {
        play.SetActive(false);
        winningCanvas.SetActive(true);
        Time.timeScale = 0f;

    }

}

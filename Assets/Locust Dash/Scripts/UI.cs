using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UI : MonoBehaviour
{
     public TMP_Text scoreText;
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

}

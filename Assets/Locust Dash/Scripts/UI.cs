using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UI : MonoBehaviour
{
     public TMP_Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

     public void updateCounter() {
        Counter.coins ++;
        scoreText.text = "Coins: " + Counter.coins.ToString();
    }
}

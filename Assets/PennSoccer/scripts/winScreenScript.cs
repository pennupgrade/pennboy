using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class winScreenScript : MonoBehaviour
{
    public int player1score;
    public int player2score;
    public string winningPlayer;
    public TextMeshProUGUI winnerText;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI scoresText;
    // Start is called before the first frame update
    void Start()
    {

        player1score = PlayerPrefs.GetInt("Player 1 Score");
        player2score = PlayerPrefs.GetInt("Player 2 Score");
        if (player1score > player2score)
        {
            winningPlayer = "Player 1";
        }
        else if (player2score > player1score)
        {
            winningPlayer = "Player 2";
        }
        else
        {
            winText.text = "TIE";
        }
        winnerText.text = winningPlayer;
        scoresText.text = player1score + " " + player2score;
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}

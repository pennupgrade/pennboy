using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreScript : ScriptableObject
{
    private int score = 0;
    public int getScore() {
        return score;
    }
    public void updateScore(int value) {
        score = score + value;
    }
}

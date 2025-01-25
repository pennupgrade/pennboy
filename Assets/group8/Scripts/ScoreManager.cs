using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    /* should keep track of all scores. all scores will be sent here */
    [SerializeField]
    private int score;
    private int highestBombLevel = 0;

    public void resetScore()
    {
        /* score may be negative, but must be integer */
        score = 0;
        highestBombLevel = 0;
    }

    public void addScore(int num)
    {
        /* score may be negative, but must be integer */
        score += num;
    }

    public int getScore()
    {
        return score;
    }

    public void reportBombLevel(int level)
    /* only called by bomb */
    {
        highestBombLevel = Mathf.Max(level, highestBombLevel);
    }

    public int getHighestBombLevel()
    /* only called by wave manager */
    {
        return highestBombLevel;
    }
}

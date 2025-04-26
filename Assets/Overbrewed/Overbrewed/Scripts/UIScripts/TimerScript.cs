using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;           // for TextMeshPro

public class TimerScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float time = 120f;
    public bool runningTime = false;
    public string displayTime;
    public float tempTime;
    public TextMeshProUGUI timeDisplay;
    public CounterScript counter;

    void Start()
    {
        runningTime = true;
        tempTime = 0;
        displayTime = "2:00";
    }

    // Update is called once per frame
    void Update()
    {
        tempTime += Time.deltaTime;

        if (runningTime)
        {
            if (tempTime >= 1f)
            {
                time--;
                displayTime = Mathf.Floor(time / 60).ToString() + ":" + (time % 60);
                tempTime = 0f;
                timeDisplay.text = displayTime;
            }
        }
        else
        {
            displayTime = "0:00";
            timeDisplay.text = displayTime;
        }
        Console.WriteLine(displayTime);
    }

}

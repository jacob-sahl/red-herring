using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float puzzleTime = 180;
    public float timeRemaining;
    private bool puzzleInProgress;

    void Start()
    {
        timeRemaining = puzzleTime;
        puzzleInProgress = true;
    }

    public void startTimer() {
        puzzleInProgress = true;
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        if (minutes < 0)
        {
            minutes = 0;
        }
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        if (seconds < 0)
        {
            seconds = 0;
        }
        GameObject UIText = GameObject.Find("TimerTextRaw");
        Text time = UIText.GetComponent<Text>();
        time.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        // Debug.Log(time.text);
    }

    void Update()
    {
        if (puzzleInProgress) {
            if (timeRemaining > 0) {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            } else {
                // gameOver();
                puzzleInProgress = false;
                timeRemaining = 0;
            }
        }
    }
}

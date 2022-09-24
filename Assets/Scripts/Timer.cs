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
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        GameObject UIText = GameObject.Find("TimerTextRaw");
        Text time = UIText.GetComponent<Text>();
        time.text = string.Format("{0:00}:{1:00}", minutes, seconds);
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

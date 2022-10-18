using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void displayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        GameObject UIText = GameObject.Find("TimerTextRaw");
        Text time = UIText.GetComponent<Text>();
        time.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void displayText(string text) {
        GameObject UIText = GameObject.Find("TimerTextRaw");
        Text time = UIText.GetComponent<Text>();
        time.text = text;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
  private float t = 0;
  public Color initialColor;
  public Color finalColor;
  public Color lerpColor;
  private float tRatio;
  // Start is called before the first frame update
  void Start()
  {
    t = 0;
    initialColor = new Color(1f, 1f * 222 / 255, 0); //FFDE00, a yellow
    finalColor = new Color(1f * 247 / 255, 0, 0); // 930000, a dark red
    lerpColor = initialColor;
  }

  public void displayTime(float timeToDisplay)
  {
    if (timeToDisplay > t) // t is the total duration time
    {
      t = timeToDisplay;
      Debug.Log("t time set: " + t);
    }
    if (t == 0) // to avoid error of dividing by 0
    {
      tRatio = 0;
    }
    else
    {
      tRatio = timeToDisplay / t;
    }
    float minutes = Mathf.FloorToInt(timeToDisplay / 60);
    float seconds = Mathf.FloorToInt(timeToDisplay % 60);
    GameObject UIText = GameObject.Find("TimerTextRaw");
    Text time = UIText.GetComponent<Text>();
    time.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    // linear interpolation to dynamically adjust color and font size based on time remaining
    lerpColor = Color.Lerp(finalColor, initialColor, tRatio); //Mathf.PingPong(tRatio, 1)); 
    time.color = lerpColor;
    // time.fontSize = Mathf.FloorToInt(Mathf.Lerp(80, 30, tRatio));

    if (tRatio < 0.25)
    {
      time.fontSize = 60;
    }
    else if (tRatio < 0.5)
    {
      time.fontSize = 45;
    }

  }
  public void displayText(string text)
  {
    GameObject UIText = GameObject.Find("TimerTextRaw");
    Text time = UIText.GetComponent<Text>();
    time.text = text;
  }
}

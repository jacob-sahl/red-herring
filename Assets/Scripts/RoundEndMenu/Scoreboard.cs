using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
  RectTransform rect;
  public GameObject player1icon;
  public GameObject player2icon;
  public GameObject player3icon;
  public GameObject player4icon;
  public GameObject player1name;
  public GameObject player2name;
  public GameObject player3name;
  public GameObject player4name;
  public float duration;
  public float pointYValue = 14.25f;
  private int pointIndex;
  private List<float> targetOffsets;
  private List<float> iconPositions;
  private float time;
  private bool animating;
  private string currAnim;
  private void Awake()
  {
    EventManager.AddListener<UIAnimationStartEvent>(onAnimationStart);
  }
  private void OnDestroy()
  {
    EventManager.RemoveListener<UIAnimationStartEvent>(onAnimationStart);
  }
  void Start()
  {
    iconPositions = new List<float> { 0f, 0f, 0f, 0f };
    targetOffsets = new List<float> { 0f, 0f, 0f, 0f };
    setIconStartPositions();
    pointIndex = 0;
    animating = false;
    
    player1name.GetComponent<TMP_Text>().text = PlayerManager.Instance.players[0].playerName;
    player2name.GetComponent<TMP_Text>().text = PlayerManager.Instance.players[1].playerName;
    player3name.GetComponent<TMP_Text>().text = PlayerManager.Instance.players[2].playerName;
    player4name.GetComponent<TMP_Text>().text = PlayerManager.Instance.players[3].playerName;
  }

  void setIconStartPositions()
  {
    RectTransform rect = player1icon.GetComponent<RectTransform>();
    int prevPoints = PlayerManager.Instance.players[0].points - PlayerManager.Instance.players[0].pointsThisRound;
    float yOffset = (pointYValue) * prevPoints;
    float newY = rect.position.y + yOffset;
    rect.position = new Vector2(rect.position.x, newY);
    iconPositions[0] = newY;

    rect = player2icon.GetComponent<RectTransform>();
    prevPoints = PlayerManager.Instance.players[1].points - PlayerManager.Instance.players[1].pointsThisRound;
    yOffset = (pointYValue) * prevPoints;
    newY = rect.position.y + yOffset;
    rect.position = new Vector2(rect.position.x, newY);
    iconPositions[1] = newY;

    rect = player3icon.GetComponent<RectTransform>();
    prevPoints = PlayerManager.Instance.players[2].points - PlayerManager.Instance.players[2].pointsThisRound;
    yOffset = (pointYValue) * prevPoints;
    newY = rect.position.y + yOffset;
    rect.position = new Vector2(rect.position.x, newY);
    iconPositions[2] = newY;

    rect = player4icon.GetComponent<RectTransform>();
    prevPoints = PlayerManager.Instance.players[3].points - PlayerManager.Instance.players[3].pointsThisRound;
    yOffset = (pointYValue) * prevPoints;
    newY = rect.position.y + yOffset;
    rect.position = new Vector2(rect.position.x, newY);
    iconPositions[3] = newY;
  }

  void onAnimationStart(UIAnimationStartEvent e)
  {
    if (e.name == "updateScore1" || e.name == "updateScore2" || e.name == "updateScore3" || e.name == "updateScore4")
    {
      currAnim = e.name;
      calculateTargets();
      animating = true;
    }
  }

  void calculateTargets()
  {
    for (var i = 0; i < 4; i++)
    {
      Debug.Log("Point Index: " + pointIndex);
      Debug.Log("i: " + i);
      targetOffsets[i] = GameController.Instance._roundEndPointStages[pointIndex][i] * pointYValue;
    }
  }

  void captureIconPositions()
  {
    iconPositions[0] = player1icon.GetComponent<RectTransform>().position.y;
    iconPositions[1] = player2icon.GetComponent<RectTransform>().position.y;
    iconPositions[2] = player3icon.GetComponent<RectTransform>().position.y;
    iconPositions[3] = player4icon.GetComponent<RectTransform>().position.y;
  }

  void endAnimation()
  {
    time = 0f;
    UIAnimationEndEvent e = new UIAnimationEndEvent();
    e.name = currAnim;
    EventManager.Broadcast(e);
    animating = false;
  }

  // Update is called once per frame
  void Update()
  {
    if (animating)
    {
      time += Time.deltaTime;
      float proportion = time / duration;

      RectTransform rect = player1icon.GetComponent<RectTransform>();
      rect.position = new Vector2(rect.position.x, iconPositions[0] + proportion * targetOffsets[0]);

      rect = player2icon.GetComponent<RectTransform>();
      rect.position = new Vector2(rect.position.x, iconPositions[1] + proportion * targetOffsets[1]);

      rect = player3icon.GetComponent<RectTransform>();
      rect.position = new Vector2(rect.position.x, iconPositions[2] + proportion * targetOffsets[2]);

      rect = player4icon.GetComponent<RectTransform>();
      rect.position = new Vector2(rect.position.x, iconPositions[3] + proportion * targetOffsets[3]);

      if (time > duration)
      {
        captureIconPositions();
        pointIndex++;
        endAnimation();
      }
    }
  }
}

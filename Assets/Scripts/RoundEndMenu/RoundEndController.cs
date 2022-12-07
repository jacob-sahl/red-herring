using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class RoundEndController : MonoBehaviour
{
  RoundEndAnimationController animator;
  public GameObject eventSystemObject;
  EventSystem eventSystem;
  RoundEndAudio audioController;
  string waitingFor;
  private void Awake()
  {
    EventManager.AddListener<UIAnimationEndEvent>(onAnimFinished);
  }

  private void OnDestroy()
  {
    EventManager.RemoveListener<UIAnimationEndEvent>(onAnimFinished);
  }
  void Start()
  {
    animator = GetComponent<RoundEndAnimationController>();
    audioController = GetComponent<RoundEndAudio>();

    eventSystem = eventSystemObject.GetComponent<EventSystem>();
    eventSystem.enabled = false;

    applyRoundEndMessages();
    TextMeshProUGUI tm = GameObject.Find("PuzzleSolvedText").gameObject.GetComponent<TextMeshProUGUI>();
    if (GameController.Instance._roundEndPuzzleComplete)
    {
      audioController.playSuccessMusic();
      tm.text = "The puzzle was solved! Well done.";
      tm.ForceMeshUpdate();
      List<string> animations = new List<string> {
        "puzzleSolvedReveal",
        "updateScore1",
        "puzzleSolvedHideTest",
        "player1Reveal",
        "updateScore2",
        "player1HideTest",
        "player2Reveal",
        "updateScore3",
        "player2HideTest",
        "player3Reveal",
        "updateScore4",
      };
      animator.startAnimationGroup(animations);
      waitingFor = "updateScore4";
    }
    else
    {
      audioController.playFailureMusic();
      tm.text = "The puzzle was not solved. How disappointing ...";
      tm.ForceMeshUpdate();
      List<string> animations = new List<string> {
        "puzzleSolvedReveal",
      };
      animator.startAnimationGroup(animations);
      waitingFor = "puzzleSolvedReveal";
    }
  }

  void onAnimFinished(UIAnimationEndEvent e)
  {
    if (e.name == waitingFor)
    {
      eventSystem.enabled = true;
    }
  }

  void applyRoundEndMessages()
  {
    List<string> messages = GameController.Instance._roundEndMessages;
    for (var i = 0; i < messages.Count; i++)
    {
      GameObject obj = GameObject.Find("PlayerText" + (i + 1)).gameObject;
      obj.SetActive(true);
      TextMeshProUGUI tm = obj.GetComponent<TextMeshProUGUI>();
      tm.text = messages[i];
      tm.ForceMeshUpdate();
    }
  }

}

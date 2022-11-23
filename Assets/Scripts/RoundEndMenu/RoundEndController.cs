using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundEndController : MonoBehaviour
{
  RoundEndAnimationController animator;
  void Start()
  {
    animator = GetComponent<RoundEndAnimationController>();
    applyRoundEndMessages();
    TextMeshProUGUI tm = GameObject.Find("PuzzleSolvedText").gameObject.GetComponent<TextMeshProUGUI>();
    if (GameController.Instance._roundEndPuzzleComplete)
    {
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
    }
    else
    {
      tm.text = "The puzzle was not solved. How disappointing ...";
      tm.ForceMeshUpdate();
      List<string> animations = new List<string> {
        "puzzleSolvedReveal",
      };
      animator.startAnimationGroup(animations);
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

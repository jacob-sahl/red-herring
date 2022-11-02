using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundEndTitle : MonoBehaviour
{
  public TextMeshProUGUI tmesh;
  void Start()
  {
    tmesh.text = "Round " + (GameController.Instance.currentRound + 1) + " Completed";
  }
}

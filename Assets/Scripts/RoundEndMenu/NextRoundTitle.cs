using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NextRoundTitle : MonoBehaviour
{
  public TextMeshProUGUI tmesh;
  void Awake()
  {
    EventManager.AddListener<LevelSetupCompleteEvent>(onLevelSetupComplete);
  }
  private void OnDestroy()
  {
    EventManager.RemoveListener<LevelSetupCompleteEvent>(onLevelSetupComplete);
  }
  void onLevelSetupComplete(LevelSetupCompleteEvent e)
  {
    updateRoundNum();
  }
  void updateRoundNum()
  {
    tmesh.text = "Round " + (GameController.Instance.currentRound + 1);
  }
}

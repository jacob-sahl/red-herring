using TMPro;
using UnityEngine;

public class NextRoundTitle : MonoBehaviour
{
    public TextMeshProUGUI tmesh;

    private void Awake()
    {
        EventManager.AddListener<LevelSetupCompleteEvent>(onLevelSetupComplete);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<LevelSetupCompleteEvent>(onLevelSetupComplete);
    }

    private void onLevelSetupComplete(LevelSetupCompleteEvent e)
    {
        updateRoundNum();
    }

    private void updateRoundNum()
    {
        tmesh.text = "Round " + (GameController.Instance.currentRound + 1);
    }
}
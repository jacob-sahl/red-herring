using TMPro;
using UnityEngine;

public class RoundEndTitle : MonoBehaviour
{
    public TextMeshProUGUI tmesh;

    private void Start()
    {
        tmesh.text = "Round " + (GameController.Instance.currentRound + 1) + " Completed";
    }
}
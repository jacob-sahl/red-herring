using TMPro;
using UnityEngine;

public class RoundEndInfo : MonoBehaviour
{
    private void Start()
    {
        gameObject.transform.Find("RoundEndText").GetComponent<TMP_Text>().text = GameController.Instance._roundEndText;
    }
}
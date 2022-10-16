using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundEndInfo : MonoBehaviour
{
    void Start()
    {
        gameObject.transform.Find("RoundEndText").GetComponent<TMP_Text>().text = GameController.Instance._roundEndText;
    }
}
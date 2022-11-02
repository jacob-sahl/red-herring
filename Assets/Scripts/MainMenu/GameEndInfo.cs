using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameEndInfo : MonoBehaviour
{
    void Start()
    {
        gameObject.transform.Find("GameEndText").GetComponent<TMP_Text>().text = GameController.Instance._gameEndText;
    }
}
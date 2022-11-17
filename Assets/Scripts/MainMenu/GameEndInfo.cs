using TMPro;
using UnityEngine;

public class GameEndInfo : MonoBehaviour
{
    private void Start()
    {
        gameObject.transform.Find("GameEndText").GetComponent<TMP_Text>().text = GameController.Instance._gameEndText;
    }
}
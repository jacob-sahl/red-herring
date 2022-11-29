using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameEndInfo : MonoBehaviour
{
  private void Start()
  {
    List<List<int>> pointsPerRound = GameController.Instance._pointsPerRound;
    List<GameObject> players = new List<GameObject>
    {
      transform.Find("Player1").gameObject,
      transform.Find("Player2").gameObject,
      transform.Find("Player3").gameObject,
      transform.Find("Player4").gameObject,
    };
    Debug.Log(players[1]);
    for (var i = 0; i < 4; i++)  // player
    {
      int total = 0;
      for (var j = 0; j < 4; j++)  // round
      {
        //Debug.Log(players[i].transform.Find("Round" + (j + 1)).gameObject.GetComponent<TextMeshProUGUI>());
        players[i].transform.Find("Round" + (j + 1)).gameObject.GetComponent<TextMeshProUGUI>().text = pointsPerRound[i][j].ToString();
        total += pointsPerRound[i][j];
      }
      players[i].transform.Find("Total").gameObject.GetComponent<TextMeshProUGUI>().text = "_____________\n" + total.ToString();
    }
  }
}
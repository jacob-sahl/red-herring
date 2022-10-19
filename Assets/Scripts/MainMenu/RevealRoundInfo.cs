using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace MainMenu
{
  public class RevealRoundInfo : MonoBehaviour
  {
    public List<GameObject> players = new List<GameObject>();

    // BANDAID
    List<string> clues = new List<string> {
      "The answer is in alphabetical order.",
      "The answer is very colourful.",
      "The answer is not secondary.",
    };

    List<string> secretObjectives = new List<string> {
      "Get the detective to look out of the window for three consecutive seconds.",
      "Get the detective to turn the typewriter upside-down.",
      "Get the detective to type 'FOOL' into the typewriter.",
    };
    //BANDAID ^

    void Start()
    {
      UpdateAllPlayers();
    }

    private void UpdateAllPlayers()
    {
      for (int i = 0; i < PlayerManager.Instance.players.Count; i++)
      {
        UpdatePlayer(i);
      }
    }
    void OnEnable()
    {
      UpdateAllPlayers();
    }

    private void UpdatePlayer(int playerID)
    {
      // BANDAID SOLUTION:

      if (playerID < players.Count)
      {
        GameObject player = players[playerID];
        var avatar = player.transform.Find("Avatar");
        PlayerController playerController = PlayerManager.Instance.getPlayer(playerID);
        avatar.GetComponent<Image>().color = playerController.color;
        if (playerController.role == PlayerRole.Detective)
        {
          player.transform.Find("Role").GetComponent<TMP_Text>().text = "Detective";
        }
        else
        {
          //BANDAID
          int i = GameController.Instance.currentSecretObjectiveAssignment.IndexOf(playerID);
          //BANDAID ^
          player.transform.Find("Role").GetComponent<TMP_Text>().text = "Informant";
          var qrCode = player.transform.Find("QRCode").gameObject;
          qrCode.SetActive(true);
          //BANDAID
          qrCode.GetComponent<QRCodeObject>().QRCodeContent = CardURLGenerator.GetCardURL("1", "window",
              clues[i], secretObjectives[i]);
          //BANDAID ^
        }
      }
    }
  }
}
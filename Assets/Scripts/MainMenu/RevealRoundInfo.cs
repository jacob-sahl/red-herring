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
      if (playerID < players.Count)
      {
        GameObject player = players[playerID];
        var avatar = player.transform.Find("Avatar");
        PlayerController playerController = PlayerManager.Instance.getPlayer(playerID);
        avatar.GetComponent<Image>().color = playerController.color;
        Debug.Log("Player: " + playerID + " Role: " + playerController.role);
        if (playerController.role == PlayerRole.Detective)
        {
          player.transform.Find("Role").GetComponent<TMP_Text>().text = "Detective";
          var qrCode = player.transform.Find("QRCode").gameObject;
          qrCode.SetActive(false);
        }
        else
        {
          SecretObjective secret = GameController.Instance.getPlayersSecretObjective(playerID);
          player.transform.Find("Role").GetComponent<TMP_Text>().text = "Informant";
          var qrCode = player.transform.Find("QRCode").gameObject;
          qrCode.SetActive(true);
          qrCode.GetComponent<QRCodeObject>().QRCodeContent = CardURLGenerator.GetCardURL("1", "window",
              secret.clue, secret.description);
          Debug.Log("Secret: " + secret.description);
        }
      }
    }
  }
}
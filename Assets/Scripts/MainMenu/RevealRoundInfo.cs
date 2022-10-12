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
                if (playerController.role == PlayerRole.Operator)
                {
                    player.transform.Find("Role").GetComponent<TMP_Text>().text = "Detective";
                }
                else
                {
                    player.transform.Find("Role").GetComponent<TMP_Text>().text = "Informant";
                    player.transform.Find("SecretText").GetComponent<TMP_Text>().text =
                        "Secret: " + playerController.instructor._goal.description;
                }
            }
        }
    }
}
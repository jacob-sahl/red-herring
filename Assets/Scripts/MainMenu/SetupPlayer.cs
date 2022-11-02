using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupPlayer : MonoBehaviour
{
  public List<GameObject> players = new List<GameObject>();
  private void Awake()
  {
    EventManager.AddListener<PlayerJoinedEvent>(onPlayerJoined);
    EventManager.AddListener<PlayerUpdateEvent>(onPlayerUpdated);
  }

  // Start is called before the first frame update
  void Start()
  {
    UpdateAllPlayers();
  }

  // Update is called once per frame
  void Update()
  {

  }

  void OnDestroy()
  {
    EventManager.RemoveListener<PlayerJoinedEvent>(onPlayerJoined);
    EventManager.RemoveListener<PlayerUpdateEvent>(onPlayerUpdated);
  }

  private void onPlayerJoined(PlayerJoinedEvent e)
  {
    Debug.Log("SetupPlayer: Player Joined");
    UpdatePlayer(e.PlayerID);
  }

  private void onPlayerUpdated(PlayerUpdateEvent e)
  {
    Debug.Log("SetupPlayer: Player Updated");
    UpdatePlayer(e.PlayerID);
  }

  private void UpdateAllPlayers()
  {
    for (int i = 0; i < PlayerManager.Instance.players.Count; i++)
    {
      UpdatePlayer(i);
    }
  }

  private void UpdatePlayer(int playerID)
  {
    if (playerID < players.Count)
    {
      GameObject player = players[playerID];
      player.transform.Find("JoinPrompt").gameObject.SetActive(false);
      var avatar = player.transform.Find("Avatar");
      avatar.GetComponent<Image>().color = PlayerManager.Instance.getPlayer(playerID).color;
      avatar.gameObject.SetActive(true);
    }
  }
}

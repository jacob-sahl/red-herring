using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
  public List<PlayerController> players = new List<PlayerController>();
  public GameObject iCursorPrefab;
  public GameObject playerPrefab;
  public static PlayerManager Instance { get; private set; }
  void Awake()
  {
    EventManager.AddListener<LevelStartEvent>(onGameStart);

    if (Instance != null && Instance != this)
    {
      Destroy(this);
    }
    else
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
  }

  private void OnDestroy()
  {
    EventManager.RemoveListener<LevelStartEvent>(onGameStart);
  }

  private int nPlayers
  {
    get { return players.Count; }
  }

  public PlayerController getPlayerByID(int id)
  {
    for (int i = 0; i < players.Count; i++)
    {
      if (players[i].playerId == id)
      {
        return players[i];
      }
    }
    return null;
  }

  // For dev purposes, fill the players so we have 4
  public void fillPlayers()
  {
    Debug.Log("Filling Players");
    while (nPlayers < 4)
    {
      GameObject newPlayer = Instantiate(playerPrefab);
      PlayerInput input = newPlayer.GetComponent<PlayerInput>();
      PlayerController p1 = getPlayerByID(0);
      if (p1.playerInput.devices.Count > 1)
      {
        // Hacky way to check if p1 is using keyboard + mouse
        input.SwitchCurrentControlScheme(new InputDevice[] { Keyboard.current, Mouse.current });
      }
      else
      {
        input.SwitchCurrentControlScheme(new InputDevice[] { Gamepad.all[0] });
      }

      Debug.Log("Devices: " + input.devices.Count + " First: " + input.devices[0].name);
    }
  }

  public int addPlayer(PlayerController player)
  {
    int playerID = nPlayers;
    players.Add(player);
    PlayerJoinedEvent e = new PlayerJoinedEvent();
    e.PlayerID = playerID;
    EventManager.Broadcast(e);
    return playerID;
  }

  void OnPlayerJoined()
  {
    Debug.Log("PlayerManager: Player Joined");
  }

  private void onGameStart(LevelStartEvent e)
  {
    Debug.Log("Game Start received by GameController");
    GetComponent<PlayerInputManager>().DisableJoining();
  }

  public PlayerController getPlayer(int playerID)
  {
    return players[playerID];
  }
}

using System.Collections.Generic;
using APIClient;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
  public List<PlayerController> players = new();
  public GameObject iCursorPrefab;
  public GameObject playerPrefab;
  public static PlayerManager Instance { get; private set; }

  private int nPlayers => players.Count;

  private void Awake()
  {
    EventManager.AddListener<LevelStartEvent>(onGameStart);
    EventManager.AddListener<GameInstanceUpdatedEvent>(OnGameInstanceUpdated);

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
    EventManager.RemoveListener<GameInstanceUpdatedEvent>(OnGameInstanceUpdated);
  }

  private void OnGameInstanceUpdated(GameInstanceUpdatedEvent e)
  {
    if (e.gameInstance == null)
    {
      return;
    }

    foreach (var player in e.gameInstance.players)
    {
      if (players.Find((controller => player.id == controller.playerId)) == null)
        this.JoinPlayer(player.name, player.id);
    }
  }

  public int GetDetectiveId()
  {
    foreach (var player in players)
    {
      if (player.role == PlayerRole.Detective)
      {
        return player.playerId;
      }
    }
    return -1;
  }

  public PlayerController getPlayerByID(int id)
  {
    for (var i = 0; i < players.Count; i++)
      if (players[i].playerId == id)
        return players[i];
    return null;
  }

  // For dev purposes, fill the players so we have 4
  public void fillPlayers(GameInstance gameInstance)
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
      GameController.Instance.gameInstance.players.Add(new Player(gameInstance.players.Count, "Player " + gameInstance.players.Count, gameInstance.id));
    }
  }

  public int GetNumInputDevicesForP1()
  {
    return getPlayerByID(0).playerInput.devices.Count;
  }

  public void JoinPlayer(string playerName, int playerId)
  {
    var newPlayer = Instantiate(playerPrefab);
    var input = newPlayer.GetComponent<PlayerInput>();
    input.SwitchCurrentControlScheme(Keyboard.current, Mouse.current);
    var playerController = newPlayer.GetComponent<PlayerController>();
    playerController.playerName = playerName;
    playerController.playerId = playerId;
    // players.Add(playerController);
  }

  public void ClearPlayers()
  {
    foreach (var player in players)
    {
      Destroy(player.gameObject);
    }
    players.Clear();
  }
  public int addPlayer(PlayerController player)
  {
    var playerID = nPlayers;
    players.Add(player);
    var e = new PlayerJoinedEvent();
    e.PlayerID = playerID;
    EventManager.Broadcast(e);
    return playerID;
  }

  private void OnPlayerJoined()
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
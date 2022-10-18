using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
  public List<PlayerController> players = new List<PlayerController>();
  public static PlayerManager Instance { get; private set; }
  void Awake() {
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

  private int nPlayers
  {
    get { return players.Count; }
  }

  void Start()
  {
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
  
  void OnPlayerJoined() {
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

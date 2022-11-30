using System.Collections.Generic;
using APIClient;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerRole
{
  Detective,
  Informant
}

public class PlayerController : MonoBehaviour
{
  public Color color;
  public int playerId;

  public PlayerRole role;
  public PlayerInput playerInput;
  public int points;
  public int pointsThisRound;
  public string playerName;
  private PlayerInputHandler _inputHandler;
  private GameController gameController;
  private GameObject iCursor;
  private PlayerManager manager;
  public Color[] playerColors = { Color.red, Color.cyan, Color.green, Color.magenta };

  private void Awake()
  {
    DontDestroyOnLoad(gameObject);
    EventManager.AddListener<LevelStartEvent>(onGameStart);
    EventManager.AddListener<LevelSetupCompleteEvent>(onLevelSetupComplete);
    EventManager.AddListener<GameInstanceUpdatedEvent>(OnGameInstanceUpdated);
    gameController = GameController.Instance;
    manager = GameController.Instance.PlayerManager;
    _inputHandler = GetComponent<PlayerInputHandler>();
    playerInput = GetComponent<PlayerInput>();
    playerId = manager.addPlayer(this);
    points = 0;
    playerName = "Player " + playerId;
    color = playerColors[playerId];
    var e = new PlayerUpdateEvent();
    e.PlayerID = playerId;
    EventManager.Broadcast(e);
  }

  private void OnDestroy()
  {
    EventManager.RemoveListener<LevelStartEvent>(onGameStart);
    EventManager.RemoveListener<LevelSetupCompleteEvent>(onLevelSetupComplete);
    EventManager.RemoveListener<GameInstanceUpdatedEvent>(OnGameInstanceUpdated);
  }
  
  private void OnGameInstanceUpdated(GameInstanceUpdatedEvent e)
  {
    GameInstance gameInstance = e.gameInstance;
    foreach (var player in gameInstance.players)
    {
      if (player.id == playerId)
      {
        if (player.name != playerName)
        {
          playerName = player.name;
          var playerUpdateEvent = new PlayerUpdateEvent();
          playerUpdateEvent.PlayerID = playerId;
          EventManager.Broadcast(e);
        }
      }
    }
  }

  private void onLevelSetupComplete(LevelSetupCompleteEvent e)
  {
    if (playerId == gameController.getCurrentDetective())
      role = PlayerRole.Detective;
    else
      role = PlayerRole.Informant;
  }

  private void onGameStart(LevelStartEvent e)
  {
    // If this is P1, make them the Detective
    if (playerId == gameController.getCurrentDetective())
    {
      GameObject.Find("Detective").GetComponent<Detective>().assignInputHandler(_inputHandler);
      playerInput.SwitchCurrentActionMap("Detective");
      GameObject.Find("PauseScreen").GetComponent<PauseScreen>().assignInputHandler(_inputHandler);
    }
    else
    {
      // Create an iCursor for this player
      // iCursor = Instantiate(manager.iCursorPrefab, GameObject.Find("Hud").transform);
      // iCursor.GetComponent<ICursorController>()._inputHandler = _inputHandler;
      // iCursor.GetComponent<ICursorController>().color = color;
      playerInput.SwitchCurrentActionMap("Informant");
    }
  }
}
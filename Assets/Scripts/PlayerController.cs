using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public enum PlayerRole
{
  Detective,
  Informant
}
public class PlayerController : MonoBehaviour
{
  private Color[] playerColors = { Color.red, Color.cyan, Color.green, Color.magenta };
  public Color color;
  public int playerId;

  public PlayerRole role;
  private GameObject iCursor;
  private PlayerInputHandler _inputHandler;
  private PlayerManager manager;
  public PlayerInput playerInput;
  private GameController gameController;
  public int points;
  public string playerName;

  private void Awake()
  {
    DontDestroyOnLoad(this.gameObject);
    EventManager.AddListener<LevelStartEvent>(onGameStart);
    EventManager.AddListener<LevelSetupCompleteEvent>(onLevelSetupComplete);
    gameController = GameController.Instance;
    manager = GameController.Instance.PlayerManager;
    _inputHandler = GetComponent<PlayerInputHandler>();
    playerInput = GetComponent<PlayerInput>();
    playerId = manager.addPlayer(this);
    points = 0;
    playerName = "Player " + playerId.ToString();
    color = playerColors[playerId];
    PlayerUpdateEvent e = new PlayerUpdateEvent();
    e.PlayerID = playerId;
    EventManager.Broadcast(e);
  }

  private void OnDestroy()
  {
    EventManager.RemoveListener<LevelStartEvent>(onGameStart);
    EventManager.RemoveListener<LevelSetupCompleteEvent>(onLevelSetupComplete);
  }

  void onLevelSetupComplete(LevelSetupCompleteEvent e)
  {
    if (playerId == gameController.getCurrentDetective())
    {
      role = PlayerRole.Detective;
    }
    else
    {
      role = PlayerRole.Informant;
    }
  }
  void onGameStart(LevelStartEvent e)
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

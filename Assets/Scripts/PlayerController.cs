using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public enum PlayerRole
{
  Operator,
  Instructor
}
public class PlayerController : MonoBehaviour
{
  private Color[] playerColors = { Color.red, Color.cyan, Color.green, Color.magenta };
  public GameObject iCursorPrefab;
  public Color color;
  private int playerId;

  public PlayerRole role;
  private GameObject iCursor;
  private PlayerInputHandler _inputHandler;
  private PlayerManager manager;
  private PlayerInput playerInput;

  // Only set when the player is an instructor
  public Instructor instructor;
  
  private void Awake()
  {
    DontDestroyOnLoad(this.gameObject);
    EventManager.AddListener<LevelStartEvent>(onGameStart);
    EventManager.AddListener<LevelSetupCompleteEvent>(onLevelSetupComplete);
    manager = GameController.Instance.PlayerManager;
    _inputHandler = GetComponent<PlayerInputHandler>();
    playerInput = GetComponent<PlayerInput>();
    playerId = manager.addPlayer(this);
    color = playerColors[playerId];
    PlayerUpdateEvent e = new PlayerUpdateEvent();
    e.PlayerID = playerId;
    EventManager.Broadcast(e);
  }

  void Start()
  {

    // // If this is P1, make them the Operator
    // if (playerId == 0)
    // {
    //   //   role = PlayerRole.Operator;
    //   GameObject.Find("Operator").GetComponent<PlayerMovement>().assignInputHandler(_inputHandler);
    //   playerInput.SwitchCurrentActionMap("Operator");
    // }
    // else
    // {
    //   //   role = PlayerRole.Instructor;
    //   // Create an iCursor for this player
    //   iCursor = Instantiate(iCursorPrefab, GameObject.Find("Hud").transform);
    //   iCursor.GetComponent<ICursorController>()._inputHandler = _inputHandler;
    //   iCursor.GetComponent<ICursorController>().color = color;
    //   playerInput.SwitchCurrentActionMap("Instructor");
    // }
  }

  void onLevelSetupComplete(LevelSetupCompleteEvent e)
  {
    if (playerId == 0)
    {
      role = PlayerRole.Operator;
    }
    else
    {
      role = PlayerRole.Instructor;
      instructor = GameController.Instance.instructors[playerId - 1];
    }
  }
  void onGameStart(LevelStartEvent e)
  {
    // If this is P1, make them the Operator
    if (playerId == 0)
    {
      GameObject.Find("Operator").GetComponent<PlayerMovement>().assignInputHandler(_inputHandler);
      playerInput.SwitchCurrentActionMap("Operator");
    }
    else
    {
      // Create an iCursor for this player
      iCursor = Instantiate(iCursorPrefab, GameObject.Find("Hud").transform);
      iCursor.GetComponent<ICursorController>()._inputHandler = _inputHandler;
      iCursor.GetComponent<ICursorController>().color = color;
      playerInput.SwitchCurrentActionMap("Instructor");
    }
  }
}

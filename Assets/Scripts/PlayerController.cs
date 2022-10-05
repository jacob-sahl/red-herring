using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
  private Color[] playerColors = { Color.red, Color.cyan, Color.green, Color.magenta };
  public GameObject iCursorPrefab;
  private Color color;
  private int playerId;
  enum PlayerRole
  {
    Operator,
    Instructor
  }
  private PlayerRole role;
  private GameObject iCursor;
  private PlayerInputHandler _inputHandler;
  private PlayerManager manager;
  private PlayerInput playerInput;
  void Start()
  {
    manager = GameController.Instance.PlayerManager;
    playerId = manager.addPlayer();
    color = playerColors[playerId];
    _inputHandler = GetComponent<PlayerInputHandler>();
    playerInput = GetComponent<PlayerInput>();

    // If this is P1, make them the Operator
    if (playerId == 0)
    {
      //   role = PlayerRole.Operator;
      GameObject.Find("Operator").GetComponent<PlayerMovement>().assignInputHandler(_inputHandler);
      playerInput.SwitchCurrentActionMap("Operator");
    }
    else
    {
      //   role = PlayerRole.Instructor;
      // Create an iCursor for this player
      iCursor = Instantiate(iCursorPrefab, GameObject.Find("Hud").transform);
      iCursor.GetComponent<ICursorController>()._inputHandler = _inputHandler;
      iCursor.GetComponent<ICursorController>().color = color;
      playerInput.SwitchCurrentActionMap("Instructor");
    }
  }
}

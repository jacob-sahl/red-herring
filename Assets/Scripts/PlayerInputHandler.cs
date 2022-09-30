using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
  [Tooltip("Sensitivity multiplier for moving the camera around")]
  public float LookSensitivity = 1f;

  [Tooltip("Used to flip the vertical input axis")]
  public bool InvertYAxis = false;

  [Tooltip("Used to flip the horizontal input axis")]
  public bool InvertXAxis = false;
  private GameController _gameController;
  private bool _interactInputWasHeld;
  private Vector2 movementInput;
  private Vector2 lookInput;


  // Start is called before the first frame update
  void Start()
  {
    _gameController = FindObjectOfType<GameController>();

    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
    lookInput = Vector2.zero;
  }

  public void OnMove(InputAction.CallbackContext context)
  {
    movementInput = context.ReadValue<Vector2>();
  }

  public Vector2 GetMoveInput()
  {
    if (CanProcessInput())
    {
      // constrain move input to a maximum magnitude of 1, otherwise diagonal movement might exceed the max move speed defined
      Vector2 move = Vector2.ClampMagnitude(movementInput, 1);

      if (InvertXAxis)
      {
        move = move * new Vector2(-1, 0);
      }
      if (InvertYAxis)
      {
        move = move * new Vector2(0, -1);
      }

      return move;
    }

    return Vector2.zero;
  }

  public void OnLook(InputAction.CallbackContext context)
  {
    lookInput = context.ReadValue<Vector2>();
  }


  public Vector2 GetLookInput()
  {
    if (CanProcessInput())
    {
      return lookInput;
    }
    return Vector2.zero;
  }

  public bool GetInteractInputDown()
  {
    if (CanProcessInput() && !_interactInputWasHeld)
    {
      if (Input.GetButtonDown(Constants.ButtonNameInteract))
      {
        _interactInputWasHeld = true;
        return true;
      }
    }
    else
    {
      _interactInputWasHeld = false;
    }

    return false;
  }

  public bool GetInteractInputHeld()
  {
    if (CanProcessInput())
    {
      return Input.GetButton(Constants.ButtonNameInteract);
    }

    return false;
  }


  public bool CanProcessInput()
  {
    return Cursor.lockState == CursorLockMode.Locked && !_gameController.gameIsEnding;
  }
}
using System;
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
  private LevelManager _levelManager;
  private bool interacted;
  private bool interactHeld;
  private bool backed;
  private bool _interactInputWasHeld;
  private Vector2 movementInput;
  private Vector2 lookInput;
  private Vector2 cursorMovement;
  private bool crouch = false;
  private bool jump = false;


  void Start()
  {
    EventManager.AddListener<LevelStartEvent>(onGameStart);
  }
  
  private void LockCursor()
  {
    Debug.Log("Locking cursor");
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
    lookInput = Vector2.zero;
  }
  
  private void UnlockCursor()
  {
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
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

  public void OnCursorMove(InputAction.CallbackContext context)
  {
    cursorMovement = context.ReadValue<Vector2>();
  }

  public Vector2 GetCursorMoveInput()
  {
    return cursorMovement;
  }

  public void OnInteract(InputAction.CallbackContext context)
  {
    interacted = context.action.triggered;
    interactHeld = context.performed;
  }

  public bool GetInteractInput()
  {
    if (CanProcessInput())
    {
      if (interacted)
      {
        interacted = false;
        return true;
      }
      return false;
    }
    return false;
  }

  public void OnInteractHeld(InputAction.CallbackContext context)
  {
    interactHeld = context.performed;
  }

  public bool GetInteractHeld()
  {
    if (CanProcessInput())
    {
      return interactHeld;
    }
    return false;
  }

  public void OnBack(InputAction.CallbackContext context)
  {
    backed = context.action.triggered;
  }

  public bool GetBackInput()
  {
    if (CanProcessInput())
    {
      if (backed)
      {
        backed = false;
        return true;
      }
      return false;
    }
    return false;
  }

  // TODO make these functions work
  public void OnCrouch(InputAction.CallbackContext context)
  {
    Debug.Log("crouch");
    crouch = context.action.triggered;
  }

  public void OnJump(InputAction.CallbackContext context)
  {
    Debug.Log("jump");
    jump = context.action.triggered;
  }

  public (bool, bool) GetCrouchAndJump()
  {
    // TODO remove these
    crouch = Input.GetKey("left ctrl");
    jump = Input.GetKeyDown("space");
    return (crouch, jump);
  }

  // public bool GetInteractInputDown()
  // {
  //   if (CanProcessInput() && !_interactInputWasHeld)
  //   {
  //     if (Input.GetButtonDown(Constants.ButtonNameInteract))
  //     {
  //       _interactInputWasHeld = true;
  //       return true;
  //     }
  //   }
  //   else
  //   {
  //     _interactInputWasHeld = false;
  //   }

  //   return false;
  // }

  // // TODO: move to New Input System
  // public bool GetInteractInputHeld()
  // {
  //   if (CanProcessInput())
  //   {
  //     return Input.GetButton(Constants.ButtonNameInteract);
  //   }

  //   return false;
  // }


  public bool CanProcessInput()
  {
    return Cursor.lockState == CursorLockMode.Locked && !GameController.Instance.IsGameEnding();
  }
  
  private void onGameStart(LevelStartEvent e)
  {
    Debug.Log("Game started Received by PlayerInputHandler");
    LockCursor();
  }
}
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
  [Tooltip("Sensitivity multiplier for moving the camera around")]
  public float LookSensitivity = 1f;

  [Tooltip("Used to flip the vertical input axis")]
  public bool InvertYAxis;

  [Tooltip("Used to flip the horizontal input axis")]
  public bool InvertXAxis;

  private bool _interactInputWasHeld;
  private LevelManager _levelManager;
  private bool backed;
  private bool crouch;
  private Vector2 cursorMovement;
  private bool interacted;
  private bool interactHeld;
  private bool jump;
  private Vector2 lookInput;
  private Vector2 movementInput;
  private bool pause;
  private PlayerInput Input;

  private void OnDisable()
  {
    Input.actions = null;
  }

  private void Start()
  {
    Input = GetComponent<PlayerInput>();
    EventManager.AddListener<LevelStartEvent>(onGameStart);
  }

  private void OnDestroy()
  {
    EventManager.RemoveListener<LevelStartEvent>(onGameStart);
  }

  private void LockCursor()
  {
    // Debug.Log("Locking cursor");
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
      var move = Vector2.ClampMagnitude(movementInput, 1);

      if (InvertXAxis) move = move * new Vector2(-1, 0);
      if (InvertYAxis) move = move * new Vector2(0, -1);

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
    if (CanProcessInput()) return lookInput;
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
    if (CanProcessInput()) return interactHeld;
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

  public void OnCrouch(InputAction.CallbackContext context)
  {
    crouch = context.action.triggered;
  }

  public void OnJump(InputAction.CallbackContext context)
  {
    jump = context.action.triggered;
  }

  public void OnPause(InputAction.CallbackContext context)
  {
    pause = context.action.triggered;
  }

  public bool GetPause()
  {
    return pause;
  }

  public (bool, bool) GetCrouchAndJump()
  {
    if (CanProcessInput())
      return (crouch, jump);
    return (false, false);
  }

  public bool CanProcessInput()
  {
    return Cursor.lockState == CursorLockMode.Locked && !GameController.Instance.IsGameEnding();
  }

  private void onGameStart(LevelStartEvent e)
  {
    // Debug.Log("Game started Received by PlayerInputHandler");
    LockCursor();
    jump = false; // Not sure why this is necessary but without it a gamepad player starts the scene jumping
  }
}
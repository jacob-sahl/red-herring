using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
  [Header("References")]
  [Tooltip("Reference to the main camera used for the player")]
  public Camera playerCamera;

  [Header("Rotation")]
  [Tooltip("Rotation speed for moving the camera")]
  public float RotationSpeed = 2f;

  [Header("Movement")]
  [Tooltip("Max movement speed")]
  public float maxSpeed = 10f;
  public float mouseDragSpeed = 5f;

  [Header("Focused")]
  public float cursorSpeed = 250f;
  public float rotateSpeed = 0.5f;

  [Tooltip(
      "Sharpness for the movement when grounded, a low value will make the player accelerate and decelerate slowly, a high value will do the opposite")]
  public float MovementSharpness = 15;
  private bool canMove;
  PlayerInputHandler _inputHandler;
  Vector2 cameraRotation = new Vector2(0, 0);
  CharacterController _controller;
  Outline _lastOutline;
  GameObject cursor;
  Vector3 cursorPosition;
  GameObject focusedObject;
  Vector3 focusRotationXAxis;
  Vector3 focusRotationYAxis;

  // Start is called before the first frame update
  void Start()
  {
    _controller = GetComponent<CharacterController>();
    cursor = GameObject.Find("DetectiveCursor");
    cursorPosition = new Vector3(Screen.width / 2, Screen.height / 2);
    canMove = true;
    EventManager.AddListener<FocusEvent>(OnFocus);
  }

  void OnFocus(FocusEvent evt)
  {
    focusedObject = GameObject.FindGameObjectWithTag(evt.ObjectTag);

    // Reset the object to face the camera
    focusedObject.transform.LookAt(playerCamera.transform.position);

    // Calculate vectors relative to the camera to serve as rotational axes
    Vector3 a = playerCamera.transform.forward;
    Vector3 b = Vector3.up;
    Vector3 c = Vector3.right;
    focusRotationXAxis = Vector3.Cross(a, b);
    focusRotationYAxis = Vector3.Cross(a, c);
  }

  void HandleCharacterMovement()
  {
    // Handle looking
    {
      Vector2 lookInput = _inputHandler.GetLookInput();
      // Process the raw lookInput 
      Vector2 lookProcessed = lookInput * RotationSpeed * Time.deltaTime;
      cameraRotation.y -= lookProcessed.y;
      // Clamp y-rotation to a range of 180 degrees
      cameraRotation.y = Mathf.Clamp(cameraRotation.y, -89f, 89f);
      cameraRotation.x += lookProcessed.x;
      // Apply only x rotation to player
      transform.localRotation = Quaternion.Euler(0f, cameraRotation.x, 0f);
      // Apply both x and y rotation to camera
      playerCamera.transform.rotation = Quaternion.Euler(cameraRotation.y, cameraRotation.x, 0f);
    }

    // Handle movement
    {
      Vector2 moveInput = _inputHandler.GetMoveInput();
      Vector3 movement = (moveInput.y * transform.forward) + (moveInput.x * transform.right);
      _controller.Move(((movement * maxSpeed) + (-transform.up)) * Time.deltaTime);
    }
  }

  void HandleFocusMovement()
  {
    // Check for exit/back
    bool exited = _inputHandler.GetBackInput();
    if (exited)
    {
      cursorPosition = new Vector3(Screen.width / 2, Screen.height / 2);
      cursor.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
      canMove = true;
      focusedObject.GetComponent<Focus>().enablePhysics();
      return;
    }
    // Observer cursor movement
    {
      Vector2 lookInput = _inputHandler.GetLookInput();
      Vector2 lookProcessed = lookInput * cursorSpeed * Time.deltaTime;
      RectTransform rect = cursor.GetComponent<RectTransform>();
      // Move the Rect of the cursor text
      float x = Mathf.Clamp(rect.anchoredPosition.x + lookProcessed.x, -(Screen.width / 2), Screen.width / 2);
      float y = Mathf.Clamp(rect.anchoredPosition.y + lookProcessed.y, -(Screen.height / 2), Screen.height / 2);
      rect.anchoredPosition = new Vector2(x, y);
      // Rect positions behave differently & weird, so recalculate for cursor
      float cx = Mathf.Clamp(cursorPosition.x + lookProcessed.x, 0, Screen.width);
      float cy = Mathf.Clamp(cursorPosition.y + lookProcessed.y, 0, Screen.height);
      cursorPosition = new Vector2(cx, cy);
    }
    // Focus object rotation 
    {
      Vector2 moveInput = _inputHandler.GetMoveInput();
      Vector2 moveProcessed = moveInput * rotateSpeed * -1;
      focusedObject.transform.RotateAround(focusedObject.transform.position, focusRotationYAxis, moveProcessed.x);
      focusedObject.transform.RotateAround(focusedObject.transform.position, focusRotationXAxis, moveProcessed.y);
    }
  }

  public void assignInputHandler(PlayerInputHandler handler)
  {
    _inputHandler = handler;
  }

  // Update is called once per frame
  void Update()
  {
    if (_inputHandler == null) return;

    RaycastHit hit;
    Ray ray = playerCamera.ScreenPointToRay(cursorPosition);
    if (Physics.Raycast(ray, out hit))
    {
      // Debug.DrawRay(ray.origin, ray.direction, Color.red, 10);
      var colliderGameObject = hit.collider.gameObject;
      var outline = colliderGameObject.GetComponent<Outline>();
      var focus = colliderGameObject.GetComponent<Focus>();

      if (outline != null)
      {
        if (_lastOutline != null && _lastOutline != outline)
        {
          _lastOutline.enabled = false;
        }
        _lastOutline = outline;
        _lastOutline.OutlineMode = Outline.Mode.OutlineAll;
        _lastOutline.OutlineWidth = 5;

        if (focus != null)
        {
          _lastOutline.OutlineColor = Color.magenta;
        }
        else
        {
          _lastOutline.OutlineColor = Color.white;
        }

        if (_inputHandler.GetInteractInput())
        {
          if (focus != null)
          {
            FocusEvent focusEvent = Events.FocusEvent;
            focusEvent.ObjectTag = colliderGameObject.tag;
            EventManager.Broadcast(focusEvent);
            canMove = false;
          }
          else
          {
            _lastOutline.OutlineColor = Color.yellow; // not sure why this isn't working
            InteractEvent interact = Events.InteractEvent;
            interact.ObjectTag = colliderGameObject.tag;
            EventManager.Broadcast(interact);
          }
        }
        _lastOutline.enabled = true;
      }
      else
      {
        if (_lastOutline != null)
        {
          _lastOutline.enabled = false;
        }
      }

      var draggable = colliderGameObject.GetComponent<Draggable>();
      if (draggable != null)
      {
        if (_inputHandler.GetInteractInput())
        {
          StartCoroutine(DragObject(colliderGameObject));
        }
      }
    }
    if (canMove)
    {
      HandleCharacterMovement();
    }
    else
    {
      HandleFocusMovement();
    }
  }

  private IEnumerator DragObject(GameObject dragObject)
  {
    float initialDistance = Vector3.Distance(dragObject.transform.position, Camera.main.transform.position);
    Rigidbody rb = dragObject.GetComponent<Rigidbody>();
    if (rb != null)
    {
      while (_inputHandler.GetInteractHeld())
      {
        Ray ray = Camera.main.ScreenPointToRay(cursorPosition);
        Vector3 direction = ray.GetPoint(initialDistance) - dragObject.transform.position;
        rb.velocity = direction * (mouseDragSpeed / rb.mass);
        //rb.AddForce(direction * mouseDragSpeed);
        yield return new WaitForFixedUpdate();
      }
    }
  }
}

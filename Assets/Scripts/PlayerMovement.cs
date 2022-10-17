using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
  public float crouchHeight = 0.5f;

  [Header("Focused")]
  public float cursorSpeed = 400f;
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
  GameObject focusedObjectPlaceholder;
  Vector3 focusRotationXAxis;
  Vector3 focusRotationYAxis;
  float cameraHeight;

  // Start is called before the first frame update
  void Start()
  {
    focusedObjectPlaceholder = new GameObject();
    _controller = GetComponent<CharacterController>();
    cursor = GameObject.Find("DetectiveCursor");
    cursorPosition = new Vector3(Screen.width / 2, Screen.height / 2);
    canMove = true;
    EventManager.AddListener<FocusEvent>(OnFocus);
    cameraHeight = playerCamera.transform.position.y;
  }

  void OnFocus(FocusEvent evt)
  {
    // Calculate vectors relative to the camera to serve as rotational axes
    focusRotationXAxis = Vector3.Cross(playerCamera.transform.forward, Vector3.up);
    focusRotationYAxis = Vector3.Cross(playerCamera.transform.forward, playerCamera.transform.right);

    focusedObject = GameObject.FindGameObjectWithTag(evt.ObjectTag);
    focusedObjectPlaceholder.transform.position = focusedObject.transform.position;
    focusedObjectPlaceholder.transform.rotation = focusedObject.transform.rotation;
    focusedObjectPlaceholder.transform.localScale = focusedObject.transform.localScale;
    Debug.Log("Focusing: " + focusedObject);

    // Reset the object to face the camera
    float distance = Vector3.Distance(focusedObject.transform.position, playerCamera.transform.position);
    focusedObject.transform.LookAt(playerCamera.transform.position);
    focusedObject.transform.Rotate(focusRotationXAxis, focusedObject.GetComponent<Focus>().defaultRotation.x, Space.World);
    focusedObject.transform.Translate((distance - focusedObject.GetComponent<Focus>().focusDistance) * -1 * playerCamera.transform.forward, Space.World);
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

    // Handle crouch and jump
    {
      (bool crouch, bool jump) = _inputHandler.GetCrouchAndJump();
      if (crouch || jump)
      {
        // check if grounded
        // TODO

        if (crouch)
        {
          playerCamera.transform.Translate(0, -crouchHeight, 0);
        }
        else
        {
          playerCamera.transform.position = new Vector3(0, cameraHeight, 0);
        }

      }
    }
  }

  void HandleFocusMovement()
  {
    // Check for exit/back
    bool exited = _inputHandler.GetBackInput();
    if (exited)
    {
      // Reset cursor to centre and exit focus
      cursorPosition = new Vector3(Screen.width / 2, Screen.height / 2);
      cursor.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
      canMove = true;
      // Put the object back where it was
      focusedObject.transform.position = focusedObjectPlaceholder.transform.position;
      focusedObject.transform.rotation = focusedObjectPlaceholder.transform.rotation;
      focusedObject.transform.localScale = focusedObjectPlaceholder.transform.localScale;
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
      // Rect positions behave differently, so recalculate for cursor
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
      // Debug.Log(colliderGameObject);
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
            interact.gameObject = colliderGameObject;
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

      if (_inputHandler.GetInteractInput())
      {
        var draggable = colliderGameObject.GetComponent<Draggable>();
        if (draggable != null)
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
      float originalMouseSpeed = RotationSpeed;
      while (_inputHandler.GetInteractHeld())
      {
        RotationSpeed = originalMouseSpeed * (1 / (1 + rb.mass));
        Ray ray = Camera.main.ScreenPointToRay(cursorPosition);
        Vector3 direction = ray.GetPoint(initialDistance) - dragObject.transform.position;
        rb.velocity = direction * mouseDragSpeed;
        //rb.AddForce(direction * mouseDragSpeed);
        yield return new WaitForFixedUpdate();
      }
      RotationSpeed = originalMouseSpeed;
    }
  }
}

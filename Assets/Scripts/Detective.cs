using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using static System.TimeZoneInfo;

[RequireComponent(typeof(CharacterController))]
public class Detective : MonoBehaviour
{
  [Header("References")]
  [Tooltip("Reference to the main camera used for the player")]
  public Camera playerCamera;

  [Header("Rotation")]
  [Tooltip("Rotation speed for moving the camera")]
  public float RotationSpeed = 1f;

  [Header("Movement")]
  [Tooltip("Max movement speed")]
  public float maxSpeed = 10f;
  public float mouseDragSpeed = 5f;
  public float crouchHeight = 0.65f;
  public float jumpHeight = 0.5f;
  public float gravity = -9.81f;
  public float gravityScale = 1;
  private float velocity;

  [Header("Focused")]
  public float cursorSpeed = 1f;
  public float objectRotateSpeed = 0.5f;

  [Tooltip(
      "Sharpness for the movement when grounded, a low value will make the player accelerate and decelerate slowly, a high value will do the opposite")]
  public float MovementSharpness = 15;
  private bool moveEnabled;
  public bool frozen;

  [Header("Audio")]
  private AudioSource audioSource;
  public AudioClip fastSteps;
  public AudioClip slowsteps;
  [Header("Color")]
  public Color focusHighlightColor;
  private bool playingStepAudio = false;
  private float stepBuffer = 0.1f;
  private float timeSinceStep = 0f;
  PlayerInputHandler _inputHandler;
  Vector2 cameraRotation = new Vector2(0, 0);
  CharacterController _controller;
  Outline _lastOutline;
  GameObject cursor;
  Vector3 cursorPosition;
  GameObject focusedObject;
  GameObject focusedObjectPlaceholder;
  GameObject lastHit;
  Vector3 focusRotationXAxis;
  Vector3 focusRotationYAxis;
  float cameraHeight;
  private Coroutine crouching = null;

  // Start is called before the first frame update
  void Start()
  {
    focusedObjectPlaceholder = new GameObject();
    _controller = GetComponent<CharacterController>();
    audioSource = GetComponent<AudioSource>();
    cursor = GameObject.Find("DetectiveCursor");
    cursorPosition = new Vector3(Screen.width / 2, Screen.height / 2);
    moveEnabled = true;
    EventManager.AddListener<FocusEvent>(OnFocus);
    cameraHeight = playerCamera.transform.localPosition.y;
  }

  private void OnDestroy()
  {
    EventManager.RemoveListener<FocusEvent>(OnFocus);
  }

  void OnFocus(FocusEvent evt)
  {
    // Calculate vectors relative to the camera to serve as rotational axes
    focusRotationXAxis = Vector3.Cross(playerCamera.transform.forward, Vector3.up);
    focusRotationYAxis = Vector3.Cross(playerCamera.transform.forward, playerCamera.transform.right);

    focusedObject = evt.gameObject;
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
      Vector2 lookProcessed = lookInput * RotationSpeed;
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
      _controller.SimpleMove(movement * maxSpeed);  // TODO manually add gravity

      // Movement audio
      {
        if (moveInput != Vector2.zero)
        {
          if (!playingStepAudio)
          {
            audioSource.clip = fastSteps;
            audioSource.Play();
            playingStepAudio = true;
          }
          if (timeSinceStep != 0f)
          {
            timeSinceStep = 0f;
          }
        }
        else if (playingStepAudio)
        {
          timeSinceStep += Time.deltaTime;
          if (timeSinceStep >= stepBuffer)
          {
            audioSource.Stop();
            playingStepAudio = false;
          }
        }
      }
    }

    // Handle crouch and jump
    {
      (bool crouch, bool jump) = _inputHandler.GetCrouchAndJump();
      if (crouch || jump)
      {
        // check if grounded
        if (_controller.isGrounded)
        {
          if (crouch && crouching == null)
          {
            crouching = StartCoroutine(Crouch());
          }
          else if (jump)
          {
            velocity = Mathf.Sqrt(jumpHeight * -2f * (gravity * gravityScale));
          }
        }
      }
    }
    velocity += gravity * gravityScale * Time.deltaTime;
    _controller.Move(new Vector3(0, velocity, 0) * Time.deltaTime);
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
      moveEnabled = true;
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
      Vector2 lookProcessed = lookInput * cursorSpeed;
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
      Vector2 moveProcessed = moveInput * objectRotateSpeed * -1;
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
    if (_inputHandler == null || frozen) return;

    RaycastHit hit;
    Ray ray = playerCamera.ScreenPointToRay(cursorPosition);
    if (Physics.Raycast(ray, out hit))
    {
      // Debug.DrawRay(ray.origin, ray.direction, Color.red, 10);
      var colliderGameObject = hit.collider.gameObject;
      bool interacted = _inputHandler.GetInteractInput();

      // Debug.Log(colliderGameObject.name);

      if (colliderGameObject != lastHit)
      {
        LookEvent evt = new LookEvent();
        evt.gameObject = colliderGameObject;
        EventManager.Broadcast(evt);
        lastHit = colliderGameObject;
      }

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
          _lastOutline.OutlineColor = Color.red;
        }
        else
        {
          _lastOutline.OutlineColor = Color.white;
        }

        if (interacted)
        {
          if (focus != null)
          {
            FocusEvent focusEvent = Events.FocusEvent;
            focusEvent.gameObject = colliderGameObject;
            EventManager.Broadcast(focusEvent);
            moveEnabled = false;
          }
          else
          {
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

      if (interacted)
      {
        var draggable = colliderGameObject.GetComponent<Draggable>();
        if (draggable != null)
        {
          StartCoroutine(DragObject(colliderGameObject));
        }
      }

    }
    if (moveEnabled)
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
        rb.AddForce(direction * mouseDragSpeed);
        yield return new WaitForFixedUpdate();
      }
      RotationSpeed = originalMouseSpeed;
    }
  }

  private IEnumerator Crouch()
  {
    float timeElapsed = 0;
    float crouchTime = 0.15f; // crouch over 0.15 seconds
    while (_inputHandler.GetCrouchAndJump().Item1)
    {
      float desiredHeight = Mathf.Lerp(cameraHeight, cameraHeight - crouchHeight, timeElapsed / crouchTime);
      playerCamera.transform.localPosition = new Vector3(0, desiredHeight, 0);
      timeElapsed += Time.deltaTime;
      yield return null;
    }

    // exit crouch
    timeElapsed = 0;
    while (timeElapsed < crouchTime)
    {
      float desiredHeight = Mathf.Lerp(cameraHeight - crouchHeight, cameraHeight, timeElapsed / crouchTime);
      playerCamera.transform.localPosition = new Vector3(0, desiredHeight, 0);
      timeElapsed += Time.deltaTime;
      yield return null;
    }

    playerCamera.transform.localPosition = new Vector3(0, cameraHeight, 0);
    crouching = null;
  }
}

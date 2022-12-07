using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using static System.TimeZoneInfo;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(CharacterController))]
public class Detective : MonoBehaviour
{
  [Header("References")]
  [Tooltip("Reference to the main camera used for the player")]
  public Camera playerCamera;
  [Tooltip("Reference to the light used for focus illumination")]
  public GameObject focusLightObject;
  HDAdditionalLightData focusLight;

  [Header("Mouse Sensitivity")]
  [Tooltip("Rotation speed for moving the camera")]
  float mouseSensitivity;
  [Tooltip("Mouse sensitivity factor when focused")]
  public float focusSensitivityMultiplier = 1.5f;


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
  public float objectRotateSpeed = 0.3f;
  public float cursorSpeed;

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
  Highlight _lastHighlight;
  GameObject _lastHover;
  GameObject cursor;
  Vector3 cursorPosition;
  GameObject focusedObject;
  GameObject focusedObjectPlaceholder;
  GameObject focusControls;
  private bool focusActive;
  GameObject lastHit;
  Vector3 focusRotationXAxis;
  Vector3 focusRotationYAxis;
  float cameraHeight;
  private Coroutine crouching = null;
  int focusPreviousLayer;

  void Start()
  {
    focusLight = focusLightObject.GetComponent<HDAdditionalLightData>();
    focusedObjectPlaceholder = new GameObject("focusedObjectPlaceholder");
    _controller = GetComponent<CharacterController>();
    audioSource = GetComponent<AudioSource>();
    cursor = GameObject.Find("DetectiveCursor");
    cursorPosition = new Vector3(Screen.width / 2, Screen.height / 2);
    moveEnabled = true;
    EventManager.AddListener<FocusEvent>(OnFocus);
    cameraHeight = playerCamera.transform.localPosition.y;
    mouseSensitivity = GameController.Instance.mouseSensitivity;
    objectRotateSpeed = GameController.Instance.objectRotationSpeed;
    focusControls = FindObjectOfType<FocusControls>().gameObject;
    focusControls.SetActive(false);
  }

  private void OnDestroy()
  {
    EventManager.RemoveListener<FocusEvent>(OnFocus);
  }

  void SetLayerRecursively(GameObject obj, int layer)
  {
    obj.layer = layer;
    foreach (Transform child in obj.transform)
      SetLayerRecursively(child.gameObject, layer);
  }

  void OnFocus(FocusEvent evt)
  {
    if (focusActive)
    {
      if (evt.gameObject == focusedObject) return;
      defocus();
    }
    // Show focus controls on-screen
    focusControls.SetActive(true);

    // Calculate vectors relative to the camera to serve as rotational axes
    focusRotationXAxis = Vector3.Cross(playerCamera.transform.forward, Vector3.up);
    focusRotationYAxis = Vector3.Cross(playerCamera.transform.forward, playerCamera.transform.right);

    focusedObject = evt.gameObject;
    focusedObjectPlaceholder.transform.position = focusedObject.transform.position;
    focusedObjectPlaceholder.transform.rotation = focusedObject.transform.rotation;
    focusedObjectPlaceholder.transform.localScale = focusedObject.transform.localScale;
    // Debug.Log("Focusing: " + focusedObject);

    // Reset the object to face the camera
    float distance = Vector3.Distance(focusedObject.transform.position, playerCamera.transform.position);
    focusedObject.transform.LookAt(playerCamera.transform.position);
    // Debug.DrawLine(focusedObject.transform.position, playerCamera.transform.position, Color.cyan, 120f, false);

    // Apply default rotation from focused object
    Focus focus = focusedObject.GetComponent<Focus>();
    focusedObject.transform.Rotate(focusRotationXAxis, focus.defaultRotation.x, Space.World);
    focusedObject.transform.Rotate(focusRotationYAxis, focus.defaultRotation.y, Space.World);
    focusedObject.transform.Rotate(playerCamera.transform.forward, focus.defaultRotation.z, Space.World);

    focusedObject.transform.Translate((distance - focus.focusDistance) * -1 * playerCamera.transform.forward, Space.World);
    // Debug.DrawLine(focusedObject.transform.position, playerCamera.transform.position, Color.green, 120f, false);

    // Apply default translation from focused object
    focusedObject.transform.Translate(focus.defaultTranslation);
    // Debug.DrawLine(focusedObject.transform.position, playerCamera.transform.position, Color.red, 120f, false);

    // Set focus light
    focusLight.intensity = focus.lightLevel;

    // Set the focused object to render on top of everything:
    focusPreviousLayer = focusedObject.layer;
    SetLayerRecursively(focusedObject, LayerMask.NameToLayer("FocusLayer"));

    focusActive = true;
    moveEnabled = false;
  }

  void defocus()
  {
    // Hide focus controls
    focusControls.SetActive(false);

    // Reset cursor to centre and exit focus
    cursorPosition = new Vector3(Screen.width / 2, Screen.height / 2);
    cursor.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

    // exit focus
    moveEnabled = true;
    focusActive = false;

    // Put the object back where it was
    focusedObject.transform.position = focusedObjectPlaceholder.transform.position;
    focusedObject.transform.rotation = focusedObjectPlaceholder.transform.rotation;
    focusedObject.transform.localScale = focusedObjectPlaceholder.transform.localScale;
    focusedObject.GetComponent<Focus>().enablePhysics();

    // Turn off focus light
    focusLight.intensity = 0f;

    // Set layer back to previous (WILL NOT WORK WITH MIXED-LAYER CHILDREN)
    SetLayerRecursively(focusedObject, focusPreviousLayer);

    // Send event
    DefocusEvent e = new DefocusEvent();
    e.gameObject = focusedObject;
    EventManager.Broadcast(e);
  }

  void HandleCharacterMovement()
  {
    // Handle looking
    {
      Vector2 lookInput = _inputHandler.GetLookInput();
      // Process the raw lookInput 
      Vector2 lookProcessed = lookInput * mouseSensitivity;
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
      defocus();
      return;
    }
    // Observer cursor movement
    {
      Vector2 lookInput = _inputHandler.GetLookInput();
      Vector2 lookProcessed = lookInput * mouseSensitivity * focusSensitivityMultiplier;
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
    mouseSensitivity = GameController.Instance.mouseSensitivity;
    cursorSpeed = GameController.Instance.mouseSensitivity;
    objectRotateSpeed = GameController.Instance.objectRotationSpeed;

    if (_inputHandler == null || frozen) return;

    RaycastHit hit;
    Ray ray = playerCamera.ScreenPointToRay(cursorPosition);

    // If we're focused, only raycast to the FocusLayer
    int layerMask;
    if (focusActive)
    {
      layerMask = LayerMask.GetMask("FocusLayer");
    }
    else
    {
      layerMask = ~0; // Everything (all 1s)
    }

    if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
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
      var highlight = colliderGameObject.GetComponent<Highlight>();
      var outline = colliderGameObject.GetComponent<Outline>();
      var focus = colliderGameObject.GetComponent<Focus>();
      var draggable = colliderGameObject.GetComponent<Draggable>();
      var typewriter = colliderGameObject.GetComponent<TypeWriter>();

      if (highlight != null)
      {
        if (_lastHighlight != null && _lastHighlight != highlight)
        {
          _lastHighlight.hideHighlight();
        }
        _lastHighlight = highlight;
        _lastHighlight.showHighlight();
      }
      else
      {
        if (_lastHighlight != null)
        {
          _lastHighlight.hideHighlight();
        }
      }

      if (interacted)
      {
        if (focus != null)
        {
          FocusEvent focusEvent = Events.FocusEvent;
          focusEvent.gameObject = colliderGameObject;
          EventManager.Broadcast(focusEvent);
        }
        else
        {
          // Debug.Log("Interacting: " + colliderGameObject.name);
          InteractEvent interact = Events.InteractEvent;
          interact.gameObject = colliderGameObject;
          EventManager.Broadcast(interact);
        }
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
      float originalMouseSpeed = mouseSensitivity;
      while (_inputHandler.GetInteractHeld())
      {
        mouseSensitivity = originalMouseSpeed * (1 / (1 + rb.mass));
        Ray ray = Camera.main.ScreenPointToRay(cursorPosition);
        Vector3 direction = ray.GetPoint(initialDistance) - dragObject.transform.position;
        rb.velocity = direction * mouseDragSpeed;
        rb.AddForce(direction * mouseDragSpeed);
        yield return new WaitForFixedUpdate();
      }
      mouseSensitivity = originalMouseSpeed;
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

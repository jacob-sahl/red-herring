using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

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

  [Tooltip(
      "Sharpness for the movement when grounded, a low value will make the player accelerate and decelerate slowly, a high value will do the opposite")]
  public float MovementSharpness = 15;
  public Vector3 CharacterVelocity;
  PlayerInputHandler _inputHandler;
  Vector2 cameraRotation = new Vector2(0, 0);
  CharacterController _controller;
  Outline _lastOutline;

  // Start is called before the first frame update
  void Start()
  {
    _controller = GetComponent<CharacterController>();
    _inputHandler = GetComponent<PlayerInputHandler>();
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
      _controller.Move(movement * maxSpeed * Time.deltaTime);
    }
  }


  private void OnFire()
  {
    //Debug.Log("Mouse Clicked");
  }

  // Update is called once per frame
  void Update()
  {
    RaycastHit hit;
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    if (Physics.Raycast(ray, out hit))
    {
      var colliderGameObject = hit.collider.gameObject;
      var outline = colliderGameObject.GetComponent<Outline>();
      if (outline != null)
      {
        if (_lastOutline != null && _lastOutline != outline)
        {
          _lastOutline.enabled = false;
        }
        _lastOutline = outline;
        _lastOutline.OutlineMode = Outline.Mode.OutlineAll;
        _lastOutline.OutlineWidth = 5;

        if (_inputHandler.GetInteractInputDown())
        {
          InteractEvent interact = Events.InteractEvent;
          interact.ObjectTag = hit.collider.gameObject.tag;
          EventManager.Broadcast(interact);
        }

        if (_inputHandler.GetInteractInputHeld())
        {
          _lastOutline.OutlineWidth = 5;
          _lastOutline.OutlineColor = Color.red;
        }
        else
        {
          _lastOutline.OutlineColor = Color.white;
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
    }

    HandleCharacterMovement();

  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")] [Tooltip("Reference to the main camera used for the player")]
    public Camera playerCamera;

    [Header("Rotation")] [Tooltip("Rotation speed for moving the camera")]
    public float RotationSpeed = 200f;

    [Header("Movement")] [Tooltip("Max movement speed")]
    public float maxSpeed = 10f;

    [Tooltip(
        "Sharpness for the movement when grounded, a low value will make the player accelerate and decelerate slowly, a high value will do the opposite")]
    public float MovementSharpness = 15;

    public Vector3 CharacterVelocity;

    PlayerInputHandler _inputHandler;
    float _cameraVerticalAngle = 0f;
    
    CharacterController _controller;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _inputHandler = GetComponent<PlayerInputHandler>();
    }

    void HandleCharacterMovement()
    {
        // horizontal character rotation
        {
            // rotate the transform with the input speed around its local Y axis
            transform.Rotate(
                new Vector3(0f, (_inputHandler.GetLookInputsHorizontal() * RotationSpeed),
                    0f), Space.Self);
        }

        // vertical camera rotation
        {
            // add vertical inputs to the camera's vertical angle
            _cameraVerticalAngle += _inputHandler.GetLookInputsVertical() * RotationSpeed;

            // limit the camera's vertical angle to min/max
            _cameraVerticalAngle = Mathf.Clamp(_cameraVerticalAngle, -89f, 89f);

            // apply the vertical angle as a local rotation to the camera transform along its right axis (makes it pivot up and down)
            playerCamera.transform.localEulerAngles = new Vector3(_cameraVerticalAngle, 0, 0);
        }

        // character movement handling
        {
            // converts move input to a worldspace vector based on our character's transform orientation
            Vector3 worldspaceMoveInput = transform.TransformVector(_inputHandler.GetMoveInput());

            // calculate the desired velocity from inputs, max speed
            Vector3 targetVelocity = worldspaceMoveInput * maxSpeed;

            // smoothly interpolate between our current velocity and the target velocity based on acceleration speed
            CharacterVelocity = Vector3.Lerp(CharacterVelocity, targetVelocity,
                MovementSharpness * Time.deltaTime);
        }

        _controller.Move(CharacterVelocity * Time.deltaTime);
    }


    private void OnFire()
    {
        //Debug.Log("Mouse Clicked");
    }
    
// Update is called once per frame
    void Update()
    {
        HandleCharacterMovement();
        if (_inputHandler.GetInteractInputDown())
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                var colliderGameObject = hit.collider.gameObject;
                Debug.Log($"HIT {colliderGameObject.name} with tag {colliderGameObject.tag}");
                InteractEvent interact = Events.InteractEvent;
                interact.ObjectTag = hit.collider.gameObject.tag;
                EventManager.Broadcast(interact);
            }
        }
    }
}

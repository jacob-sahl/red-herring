using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    // Start is called before the first frame update
    void Start()
    {
        _gameController = FindObjectOfType<GameController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public Vector3 GetMoveInput()
    {
        if (CanProcessInput())
        {
            Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0f,
                Input.GetAxisRaw("Vertical"));

            // constrain move input to a maximum magnitude of 1, otherwise diagonal movement might exceed the max move speed defined
            move = Vector3.ClampMagnitude(move, 1);

            return move;
        }

        return Vector3.zero;
    }

    public float GetLookInputsHorizontal()
    {
        return GetMouseOrStickLookAxis(Constants.MouseAxisNameHorizontal, Constants.AxisNameJoystickLookHorizontal);
    }

    public float GetLookInputsVertical()
    {
        return GetMouseOrStickLookAxis(Constants.MouseAxisNameVertical, Constants.AxisNameJoystickLookVertical);
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

    public bool CanProcessInput()
    {
        return Cursor.lockState == CursorLockMode.Locked && !_gameController.gameIsEnding;
    }

    float GetMouseOrStickLookAxis(string mouseInputName, string stickInputName)
    {
        if (CanProcessInput())
        {
            // Check if this look input is coming from the mouse
            bool isGamepad = Input.GetAxis(stickInputName) != 0f;
            float i = isGamepad ? Input.GetAxis(stickInputName) : Input.GetAxisRaw(mouseInputName);

            // handle inverting vertical input
            if (InvertYAxis)
                i *= -1f;

            // apply sensitivity multiplier
            i *= LookSensitivity;

            if (isGamepad)
            {
                // since mouse input is already deltaTime-dependant, only scale input with frame time if it's coming from sticks
                i *= Time.deltaTime;
            }
            else
            {
                // reduce mouse input amount to be equivalent to stick movement
                i *= 0.01f;
            }

            return i;
        }

        return 0f;
    }
}
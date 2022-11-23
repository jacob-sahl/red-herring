using UnityEngine;
using UnityEngine.InputSystem;

public class InstructorInputHandler : MonoBehaviour
{
    private Vector2 cursorMovement;

    public void OnCursorMove(InputAction.CallbackContext context)
    {
        cursorMovement = context.ReadValue<Vector2>();
    }

    public Vector2 GetCursorMove()
    {
        return cursorMovement;
    }
}
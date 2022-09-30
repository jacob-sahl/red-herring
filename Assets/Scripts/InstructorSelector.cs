using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InstructorSelector : MonoBehaviour
{

  private ICursorController[] controllers;
  private int currentInstructor;
  private bool toggled = false;

  private InstructorInputHandler _inputHandler;

  void Start()
  {
    currentInstructor = 0;
    controllers = FindObjectsOfType<ICursorController>();
    _inputHandler = GetComponent<InstructorInputHandler>();
    selectInstructor();
  }

  private void selectInstructor()
  {
    for (int i = 0; i < controllers.Length; i++)
    {
      if (i == currentInstructor)
      {
        controllers[i]._dev_handling = true;
        Debug.Log("Enabling cursor: " + i);
      }
      else
      {
        controllers[i]._dev_handling = false;
        Debug.Log("Disabling cursor: " + i);
      }
    }
  }

  public void OnToggle(InputAction.CallbackContext context)
  {
    Debug.Log("Toggled");
    toggled = context.action.triggered;
  }

  public int GetCurrentInstructor()
  {
    return currentInstructor;
  }

  void Update()
  {
    if (toggled)
    {
      toggled = false;
      currentInstructor = (currentInstructor + 1) % controllers.Length;
      selectInstructor();
    }
  }
}

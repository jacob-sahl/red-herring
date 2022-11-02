using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class FocusControls : MonoBehaviour
{
  TextMeshProUGUI rotateText;
  TextMeshProUGUI dropText;
  void Start()
  {
    rotateText = transform.Find("RotateText").GetComponent<TextMeshProUGUI>();
    rotateText.text = "Rotate Object: ";
    dropText = transform.Find("DropText").GetComponent<TextMeshProUGUI>();
    dropText.text = "Drop Object: ";
    int detectiveID = GameController.Instance.getCurrentDetective();
    PlayerController detective = PlayerManager.Instance.getPlayerByID(detectiveID);

    string controls = detective.playerInput.currentControlScheme;

    foreach (var binding in detective.playerInput.currentActionMap.bindings)
    {
      if (binding.path.Contains(controls))
      {
        if (binding.action == "Back")
        {
          dropText.text += binding.ToDisplayString();
        }
        // if (binding.action == "Move")
        // {
        //   rotateText.text += binding.ToDisplayString();
        // }
      }
    }
    // BANDAID
    if (controls == "Keyboard")
    {
      rotateText.text += "WASD or Arrow Keys";
    }
    else
    {
      rotateText.text += "Left Stick";
    }
    // BANDAID ^
    gameObject.SetActive(false);
  }
}

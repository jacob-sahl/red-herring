using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.EventSystems;

public class FocusControls : MonoBehaviour
{
    TextMeshProUGUI rotateText;
    TextMeshProUGUI dropText;

    void Start()
    {
        rotateText = transform.Find("RotateText").GetComponent<TextMeshProUGUI>();
        dropText = transform.Find("DropText").GetComponent<TextMeshProUGUI>();
        int detectiveID = GameController.Instance.getCurrentDetective();
        PlayerController detective = PlayerManager.Instance.getPlayerByID(detectiveID);

        string controls = detective.playerInput.currentControlScheme;

        // foreach (var binding in detective.playerInput.currentActionMap.bindings)
        // {
        //   if (binding.path.Contains(controls))
        //   {
        //     if (binding.action == "Back")
        //     {
        //       dropText.text += binding.ToDisplayString();
        //     }
        //     // if (binding.action == "Move")
        //     // {
        //     //   rotateText.text += binding.ToDisplayString();
        //     // }
        //   }
        // }
        // BANDAID
        if (controls == "Keyboard")
        {
            dropText.text = "E";
            rotateText.text = "WASD / \n Arrow Keys";
        }
        else
        {
            dropText.text = "East Button ( B / O )";
            rotateText.text = "Left Stick";
        }
    }
}
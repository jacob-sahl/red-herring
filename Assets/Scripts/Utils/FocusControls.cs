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

        if (controls == "Keyboard")
        {
            dropText.text = Constants.keyboardControls["Drop Object"];
            rotateText.text = Constants.keyboardControls["Movement"];
    }
        else
        {
            dropText.text = Constants.gamepadControls["Drop Object"];
            rotateText.text = Constants.gamepadControls["Movement"];
        }
    }
}
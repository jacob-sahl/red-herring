using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class ControlsDisplay : MonoBehaviour
{
  TextMeshProUGUI controlsText;
  public string controlScheme = "";
  void Start()
  {
    controlsText = transform.Find("ControlsText").GetComponent<TextMeshProUGUI>();

    if (controlScheme == "")
    {
      int detectiveID = GameController.Instance.getCurrentDetective();
      PlayerController detective = PlayerManager.Instance.getPlayerByID(detectiveID);
      Debug.Log(detective);

      controlScheme = detective.playerInput.currentControlScheme;
    }
    Dictionary<string, string> controlsMap = null;
    if (controlScheme == "Keyboard")
    {
      controlsMap = Constants.keyboardControls;
      controlsText.text += "Keyboard Controls \n \n";
    }
    else if (controlScheme == "Gamepad")
    {
      controlsMap = Constants.gamepadControls;
      controlsText.text += "Gamepad Controls \n \n";
    }
    foreach (KeyValuePair<string, string> control in controlsMap)
    {
      controlsText.text += control.Key + ": " + control.Value + "\n";
    }
  }
}
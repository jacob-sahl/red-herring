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
     // Mouse, WASD/ Arrow Keys, Left Click, Ctrl, Space, Tab
      if (control.Value == "E")
      {
        controlsText.text += control.Key + ": " + "<sprite=1>" + "\n";
      }
      else if (control.Value == "P")
      {
        controlsText.text += control.Key + ": " + "<sprite=2>" + "\n";
      }
      else if (control.Value == "Ctrl")
      {
        controlsText.text += control.Key + ": " + "<sprite=0>" + "\n";
      }
      else if (control.Value == "Space")
      {
        controlsText.text += control.Key + ": " + "<sprite=4>" + "\n";
      }
      else if (control.Value == "Tab")
      {
        controlsText.text += control.Key + ": " + "<sprite=5>" + "\n";
      }
      else if (control.Value == "WASD / Arrow Keys")
      {
        controlsText.text += control.Key + ": " + "<sprite=6>/<sprite=7>" + "\n";
      }
      else 
      {
        controlsText.text += control.Key + ": " + control.Value + "\n";
      }
    }
  }
}
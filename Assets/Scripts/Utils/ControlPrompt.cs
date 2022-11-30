using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ControlPrompt : MonoBehaviour
{
  public string control;
  TextMeshProUGUI promptText;
  public float timeDelay = 0f;
  public float timeLimit = 5f;
  private float time = 0;
  public float objectY = 150;

  // Start is called before the first frame update
  void Start()
  {
    Debug.Log(objectY);
    promptText = transform.Find("PromptText").GetComponent<TextMeshProUGUI>();

    int detectiveID = GameController.Instance.getCurrentDetective();
    PlayerController detective = PlayerManager.Instance.getPlayerByID(detectiveID);
    Debug.Log(detective);

    string controlScheme = detective.playerInput.currentControlScheme;

    Dictionary<string, string> controlsMap = null;
    if (controlScheme == "Keyboard")
    {
      controlsMap = Constants.keyboardControls;
    }
    else if (controlScheme == "Gamepad")
    {
      controlsMap = Constants.gamepadControls;
    }
        // Mouse, WASD/ Arrow Keys, Left Click, Ctrl, Space, Tab
        if (controlsMap[control] == "E")
        {
            promptText.text += control + ": " + "<sprite=1>" + "\n";
        }
        else if (controlsMap[control] == "P")
        {
            promptText.text += control + ": " + "<sprite=2>" + "\n";
        }
        else if (controlsMap[control] == "Ctrl")
        {
            promptText.text += control + ": " + "<sprite=0>" + "\n";
        }
        else if (controlsMap[control] == "Space")
        {
            promptText.text += control + ": " + "<sprite=4>" + "\n";
        }
        else if (controlsMap[control] == "Tab")
        {
            promptText.text += control + ": " + "<sprite=5>" + "\n";
        }
        else if (controlsMap[control] == "WASD / Arrow Keys")
        {
            promptText.text += control + ": " + "<sprite=6>/<sprite=7>" + "\n";
        }
        else
        {
            promptText.text = control + ": " + controlsMap[control];
        }
  }

  // Update is called once per frame
  void Update()
  {
    // TODO allow fadeout option
    time += Time.deltaTime;
    if (time > timeLimit + timeDelay)
    {
      Destroy(gameObject);
    }
    else if (time > timeDelay & time <= timeDelay + 1)
    {
      transform.Translate(new Vector3(0, objectY * Time.deltaTime), 0);
    }
  }
}

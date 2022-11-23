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
    promptText.text = control + ": " + controlsMap[control];
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

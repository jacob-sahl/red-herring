using TMPro;
using UnityEngine.EventSystems;

public class FocusControls : MonoBehaviour
{
  TextMeshProUGUI rotateText;
  TextMeshProUGUI dropText;
  public float hidePosition = -300;
  public float displayPosition = 0;
  public float transitionTime = 1.5f;

  void Start()
  {
    rotateText = transform.Find("RotateText").GetComponent<TextMeshProUGUI>();
    dropText = transform.Find("DropText").GetComponent<TextMeshProUGUI>();
    int detectiveID = GameController.Instance.getCurrentDetective();
    PlayerController detective = PlayerManager.Instance.getPlayerByID(detectiveID);

    private void Start()
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
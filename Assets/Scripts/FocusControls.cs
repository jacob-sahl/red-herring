using TMPro;
using UnityEngine;

public class FocusControls : MonoBehaviour
{
    private TextMeshProUGUI dropText;
    private TextMeshProUGUI rotateText;

    private void Start()
    {
        rotateText = transform.Find("RotateText").GetComponent<TextMeshProUGUI>();
        rotateText.text = "Rotate Object: ";
        dropText = transform.Find("DropText").GetComponent<TextMeshProUGUI>();
        dropText.text = "Drop Object: ";
        var detectiveID = GameController.Instance.getCurrentDetective();
        var detective = PlayerManager.Instance.getPlayerByID(detectiveID);

        var controls = detective.playerInput.currentControlScheme;

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
            dropText.text += "E";
            rotateText.text += "WASD or Arrow Keys";
        }
        else
        {
            dropText.text += "Right Button (B)";
            rotateText.text += "Left Stick";
        }
        // BANDAID ^
        // gameObject.SetActive(false);
    }
}
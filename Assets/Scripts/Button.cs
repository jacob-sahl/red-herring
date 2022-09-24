using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
//using UnityEngine.Windows;

public class Button : MonoBehaviour
{
    bool mousePressed = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !mousePressed) {
            mousePressed = true;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "Button")
                {
                    Debug.Log("Button Pressed");
                    pressButton(hit.collider.gameObject);
                }
            }
        } else {
            mousePressed = false;
        }
    }

    void pressButton(GameObject button)
    {
        switch (button.name)
        {
            case "skull":
                //players[1].win = True;
                Debug.Log("Player 1 Wins");
                break;
            case "reset":
                //inputs = "";
                Debug.Log(" ");
                break;
            case "code1":
                Debug.Log("+1");
                //inputs = inputs + "1";
                break;
        }
    }
}

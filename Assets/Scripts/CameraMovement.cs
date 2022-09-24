using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float sensitivity = 1000;
    
    public PuzzleManager puzzleManager;
    
    bool mousePressed = false;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float Yrotation = Input.GetAxis("Mouse Y");
        transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime * -1, 0.0f, 0.0f));

        if (Input.GetMouseButtonDown(0) && !mousePressed)
        {
            mousePressed = true;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log($"HIT {hit.collider.gameObject.name}");
                switch (hit.collider.gameObject.tag)
                {
                    case "key1":
                        puzzleManager.ButtonPressed(ButtonType.Key1);
                        break;
                    case "key2":
                        puzzleManager.ButtonPressed(ButtonType.Key2);
                        break;
                    case "key3":
                        puzzleManager.ButtonPressed(ButtonType.Key3);
                        break;
                    case "key4":
                        puzzleManager.ButtonPressed(ButtonType.Key4);
                        break;
                    case "reset":
                        puzzleManager.ButtonPressed(ButtonType.Reset);
                        break;
                    case "green":
                        puzzleManager.ButtonPressed(ButtonType.Green);
                        break;
                }
            }
        }
        else
        {
            mousePressed = false;
        }
        
    }
}

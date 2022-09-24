using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float sensitivity = 1000;
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
                if (hit.collider.gameObject.tag == "Button")
                {
                    Debug.Log(string.Format("Button {0} Pressed", hit.collider.gameObject.GetComponent<Button>().id));
                    // gameManager buttonPressed(id);
                }
            }
        }
        else
        {
            mousePressed = false;
        }
        
    }
}

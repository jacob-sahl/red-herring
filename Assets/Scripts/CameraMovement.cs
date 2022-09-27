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
        transform.Rotate(new Vector3(Mathf.Clamp(Yrotation * sensitivity * Time.deltaTime * -1, -89f, 89f), 0.0f, 0.0f));

        if (Input.GetMouseButtonDown(0) && !mousePressed)
        {
            mousePressed = true;
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log($"HIT {hit.collider.gameObject.name} with tag {hit.collider.gameObject.tag}");
                puzzleManager.ButtonPressed(hit.collider.gameObject.tag);
            }
        }
        else
        {
            mousePressed = false;
        }
        
    }
}

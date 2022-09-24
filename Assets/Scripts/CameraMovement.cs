using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float sensitivity = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float Yrotation = Input.GetAxis("Mouse Y");
        transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime * -1, 0.0f, 0.0f));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private float xMovement;
    private float yMovement;
    public float speed = 1;
    public float sensitivity = 10;
    private float yRotation = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        xMovement = movementVector.x;
        yMovement = movementVector.y;
    }

    private void OnFire()
    {
        Debug.Log("Mouse Clicked");
    }

    // Update is called once per frame
    void Update()
    {
        float Yrotation = Input.GetAxis("Mouse X");
        yRotation += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
        if (transform.position.y != 2)
        {
            transform.position = new Vector3(transform.position.x, 2, transform.position.z);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddRelativeForce(new Vector3(xMovement, 0, yMovement) * speed);
        if (transform.position.y != 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }
}

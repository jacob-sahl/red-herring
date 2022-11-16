using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ICursorController : MonoBehaviour
{
    private RectTransform _rect;
    public PlayerInputHandler _inputHandler;
    public bool _dev_handling = true;
    public Color color;
    //public float initCursorSpeed;
    private float cursorSpeed;

    void Start()
    {
        _rect = GetComponent<RectTransform>();
        //cursorSpeed = initCursorSpeed;
        cursorSpeed = GameController.Instance.mouseSensitivity;
        Debug.Log("cursor speed:"+ cursorSpeed);
    }

    private void handleCursorMove()
    {
        Vector2 lookInput = _inputHandler.GetCursorMoveInput();
        float x = Mathf.Clamp(_rect.anchoredPosition.x + (lookInput.x * cursorSpeed), -(Screen.width / 2), Screen.width / 2);
        float y = Mathf.Clamp(_rect.anchoredPosition.y + (lookInput.y * cursorSpeed), -(Screen.height / 2), Screen.height / 2);
        _rect.anchoredPosition = new Vector2(x, y);
        //Debug.Log("cursor speed:" + cursorSpeed);
    }

    public Vector3 GetPosition()
    {
        return _rect.transform.position;
    }

    void Update()
    {
        cursorSpeed = GameController.Instance.mouseSensitivity;
        handleCursorMove();
    }
}

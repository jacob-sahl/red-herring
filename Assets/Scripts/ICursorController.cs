using UnityEngine;

public class ICursorController : MonoBehaviour
{
    public PlayerInputHandler _inputHandler;
    public bool _dev_handling = true;
    public Color color;

    private RectTransform _rect;

    //public float initCursorSpeed;
    private float cursorSpeed;

    private void Start()
    {
        _rect = GetComponent<RectTransform>();
        //cursorSpeed = initCursorSpeed;
        cursorSpeed = GameController.Instance.mouseSensitivity;
        Debug.Log("cursor speed:" + cursorSpeed);
    }

    private void Update()
    {
        handleCursorMove();
    }

    private void handleCursorMove()
    {
        var lookInput = _inputHandler.GetCursorMoveInput();
        var x = Mathf.Clamp(_rect.anchoredPosition.x + lookInput.x * cursorSpeed, -(Screen.width / 2),
            Screen.width / 2);
        var y = Mathf.Clamp(_rect.anchoredPosition.y + lookInput.y * cursorSpeed, -(Screen.height / 2),
            Screen.height / 2);
        _rect.anchoredPosition = new Vector2(x, y);
        //Debug.Log("cursor speed:" + cursorSpeed);
    }

    public Vector3 GetPosition()
    {
        return _rect.transform.position;
    }
}
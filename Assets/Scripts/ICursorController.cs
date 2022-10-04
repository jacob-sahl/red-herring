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

  void Start()
  {
    _rect = GetComponent<RectTransform>();
  }

  private void handleCursorMove()
  {
    Vector2 lookInput = _inputHandler.GetCursorMoveInput();
    float x = Mathf.Clamp(_rect.anchoredPosition.x + lookInput.x, -(Screen.width / 2), Screen.width / 2);
    float y = Mathf.Clamp(_rect.anchoredPosition.y + lookInput.y, -(Screen.height / 2), Screen.height / 2);
    _rect.anchoredPosition = new Vector2(x, y);
  }

  public Vector3 GetPosition()
  {
    return _rect.transform.position;
  }

  void Update()
  {
    // if (!_dev_handling) return;
    handleCursorMove();
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICursorController : MonoBehaviour
{
  private RectTransform _rect;
  private InstructorInputHandler _inputHandler;

  public bool _dev_handling = true;

  void Start()
  {
    _rect = GetComponent<RectTransform>();
    _inputHandler = GameObject.Find("Instructors").GetComponent<InstructorInputHandler>();
  }

  private void handleCursorMove()
  {
    Vector2 lookInput = _inputHandler.GetCursorMove();

    float x = Mathf.Clamp(_rect.anchoredPosition.x + lookInput.x, -(Screen.width / 2), Screen.width / 2);
    float y = Mathf.Clamp(_rect.anchoredPosition.y + lookInput.y, -(Screen.height / 2), Screen.height / 2);
    _rect.anchoredPosition = new Vector2(x, y);
  }

  void Update()
  {
    if (!_dev_handling) return;
    handleCursorMove();
  }
}

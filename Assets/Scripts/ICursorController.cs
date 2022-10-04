using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ICursorController : MonoBehaviour
{
  private RectTransform _rect;
  private InstructorInputHandler _inputHandler;
  public int iID;
  public bool _dev_handling = true;
  private Instructor instructor;

  void Start()
  {
    _rect = GetComponent<RectTransform>();
    _inputHandler = GameObject.Find("Instructors").GetComponent<InstructorInputHandler>();
    instructor = GameObject.Find("Instructor" + iID).GetComponent<Instructor>();
  }

  private void handleCursorMove()
  {
    Vector2 lookInput = _inputHandler.GetCursorMove();

    float x = Mathf.Clamp(_rect.anchoredPosition.x + lookInput.x, -(Screen.width / 2), Screen.width / 2);
    float y = Mathf.Clamp(_rect.anchoredPosition.y + lookInput.y, -(Screen.height / 2), Screen.height / 2);
    _rect.anchoredPosition = new Vector2(x, y);
  }

  public Instructor GetInstructor()
  {
    return instructor;
  }

  public Vector3 GetPosition()
  {
    return _rect.transform.position;
  }

  void Update()
  {
    if (!_dev_handling) return;
    handleCursorMove();
  }
}

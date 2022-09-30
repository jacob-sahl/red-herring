using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICursorController : MonoBehaviour
{
  private RectTransform _rect;
  private PlayerInputHandler _inputHandler;
  // Start is called before the first frame update
  void Start()
  {
    _rect = GetComponent<RectTransform>();
    // position = new Vector2(transform.position.x, transform.position.y);
    _inputHandler = GetComponent<PlayerInputHandler>();
  }

  private void handleCursorMove()
  {
    Vector2 lookInput = _inputHandler.GetLookInput();
    float x = Mathf.Clamp(lookInput.x, -(Screen.width / 2), Screen.width / 2);
    float y = Mathf.Clamp(lookInput.y, -(Screen.height / 2), Screen.height / 2);
    _rect.position = new Vector3(x, y, 0);
  }

  // Update is called once per frame
  void Update()
  {
    handleCursorMove();
  }
}

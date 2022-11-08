using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextWobble : MonoBehaviour
{
  private bool wobbling;
  void Awake()
  {
    wobbling = false;
  }
  public void setWobbling(bool state)
  {
    wobbling = state;
  }
  public bool getWobbling()
  {
    return wobbling;
  }
}

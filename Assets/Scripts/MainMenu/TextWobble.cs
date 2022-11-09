using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TextReveal))]
// This isn't ideal architecture -- this class is really just a flag and the behaviour is actually
// controlled by TextReveal.cs, but it works
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

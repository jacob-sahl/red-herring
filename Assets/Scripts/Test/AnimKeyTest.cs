using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimKeyTest : MonoBehaviour
{
  private Animator animator;
  float time;
  public int offset;
  public int period;
  bool pass;
  // Start is called before the first frame update
  void Start()
  {
    animator = GetComponent<Animator>();
    time = 0f;
    pass = true;
  }

  // Update is called once per frame
  void Update()
  {
    time += Time.deltaTime;
    if (time > offset && pass)
    {
      // Debug.Log("triggering");
      animator.SetTrigger("KeyPress");
      pass = false;
    }
    if (time > period)
    {
      time = 0f;
      pass = true;
    }
  }
}

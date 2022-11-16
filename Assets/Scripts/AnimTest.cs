using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTest : MonoBehaviour
{
  private Animator animator;
  float time;
  void Start()
  {
    animator = GetComponent<Animator>();
    time = 0f;
  }

  // Update is called once per frame
  void Update()
  {
    time += Time.deltaTime;
    if (time > 3f)
    {
      animator.SetTrigger("IncorrectInput");
      time = 0f;
    }
  }
}

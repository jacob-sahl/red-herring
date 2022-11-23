using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTest : MonoBehaviour
{
  private Animator animator;
  float time;
  int i;
  void Start()
  {
    animator = GetComponent<Animator>();
    time = 0f;
    i = 0;
  }

  // Update is called once per frame
  void Update()
  {
    time += Time.deltaTime;
    if (time > 3f)
    {
      if (i % 3 == 0)
      {
        animator.SetTrigger("CorrectInput");
      }
      else if (i % 3 == 1)
      {
        animator.SetTrigger("KeyPress");
      }
      else
      {
        animator.SetTrigger("IncorrectInput");
      }
      i++;
      // Debug.Log(animator.GetCurrentAnimatorStateInfo(0).name);
      time = 0f;
      string[] states = { "Idle", "Success", "Failure" };
      foreach (var state in states)
      {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(state))
        {
          Debug.Log("ANIM STATE: " + state);
        }
      }
    }
  }
}

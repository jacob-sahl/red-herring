using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeElementsOut : MonoBehaviour
{
  public string animationName;
  public float duration;
  CanvasGroup group;
  bool animating;
  float time;
  void Awake()
  {
    animating = false;
    time = 0f;
    EventManager.AddListener<UIAnimationStartEvent>(onAnimationStart);
  }
  private void Start()
  {
    group = GetComponent<CanvasGroup>();
  }

  private void OnDestroy()
  {
    EventManager.RemoveListener<UIAnimationStartEvent>(onAnimationStart);
  }

  void onAnimationStart(UIAnimationStartEvent e)
  {
    if (e.name == animationName)
    {
      animating = true;
    }
  }

  void endAnimation()
  {
    UIAnimationEndEvent e = new UIAnimationEndEvent();
    e.name = animationName;
    EventManager.Broadcast(e);
    animating = false;
    time = 0f;
  }

  void Update()
  {
    if (animating)
    {
      time += Time.deltaTime;
      group.alpha = Mathf.Clamp01(1 - time / duration);
      if (time >= duration)
      {
        endAnimation();
      }
    }
  }
}

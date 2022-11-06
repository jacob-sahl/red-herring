using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyRectSize : MonoBehaviour
{
  public string animationName;
  public float duration;
  RectTransform rect;
  Vector2 initialSize;
  public Vector2 targetSize;
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
    rect = GetComponent<RectTransform>();
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
      initialSize = rect.rect.size;
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
      if (time < duration)
      {
        time += Time.deltaTime;
        rect.sizeDelta = Vector2.Lerp(initialSize, targetSize, time / duration);
      }
      else
      {
        endAnimation();
      }
    }
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
  public string animationName;
  RectTransform rect;
  Vector3 initPosition;
  void Start()
  {
    rect = GetComponent<RectTransform>();
    initPosition = rect.anchoredPosition;
  }

  private void Awake()
  {
    EventManager.AddListener<UIAnimationStartEvent>(onAnim);
  }

  private void OnDestroy()
  {
    EventManager.RemoveListener<UIAnimationStartEvent>(onAnim);
  }

  void onAnim(UIAnimationStartEvent e)
  {
    if (e.name == animationName)
    {
      resetPosition();
    }
  }

  void resetPosition()
  {
    rect.anchoredPosition = initPosition;
    UIAnimationEndEvent evt = new UIAnimationEndEvent();
    evt.name = animationName;
    EventManager.Broadcast(evt);
  }
}

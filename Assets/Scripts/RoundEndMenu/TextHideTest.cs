using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextHideTest : MonoBehaviour
{
  public string animationName;
  TextWobble wobble;
  void Awake()
  {
    EventManager.AddListener<UIAnimationStartEvent>(onAnimationStart);
  }
  private void Start()
  {
    wobble = GetComponent<TextWobble>();
  }

  private void OnDestroy()
  {
    EventManager.RemoveListener<UIAnimationStartEvent>(onAnimationStart);
  }

  void onAnimationStart(UIAnimationStartEvent e)
  {
    if (e.name == animationName)
    {
      // I have no idea why this is necessary but TextHiding doesn't work on RoundEndText for some reason
      if (wobble != null) wobble.setWobbling(false);
      TextMeshProUGUI mesh = GetComponent<TextMeshProUGUI>();
      mesh.ForceMeshUpdate();
      mesh.text = "";
      gameObject.SetActive(false);
      endAnimation();
    }

  }

  void endAnimation()
  {
    UIAnimationEndEvent e = new UIAnimationEndEvent();
    e.name = animationName;
    EventManager.Broadcast(e);
  }
}

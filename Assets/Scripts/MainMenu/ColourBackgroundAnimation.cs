using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColourBackgroundAnimation : MonoBehaviour
{
  string animationName = "colourBackgroundFade";
  public Color initialBackground;
  public Color targetBackground;
  public float initialHold;
  public float firstTransitionTime;
  public float secondaryHold;
  public float secondTransitionTime;
  Image img;
  CanvasGroup group;
  bool animating;
  float time;
  float elapsed;
  float phaseTwoElapsed;
  bool started;
  bool secondStarted;
  void Awake()
  {
    animating = false;
    EventManager.AddListener<UIAnimationStartEvent>(onAnimStart);
  }
  private void OnDestroy()
  {
    EventManager.RemoveListener<UIAnimationStartEvent>(onAnimStart);
  }

  private void Start()
  {
    time = 0f;
    started = false;
    secondStarted = false;
    img = GetComponent<Image>();
    group = GetComponent<CanvasGroup>();
  }

  void onAnimStart(UIAnimationStartEvent e)
  {
    if (e.name == animationName)
    {
      animating = true;
    }
  }

  void Update()
  {
    if (animating)
    {
      if (time > initialHold)
      {
        if (!started)
        {
          elapsed = 0f;
          started = true;
        }
        img.color = Color.Lerp(initialBackground, targetBackground, elapsed / firstTransitionTime);
        elapsed += Time.deltaTime;
        if (elapsed > firstTransitionTime + secondaryHold)
        {
          if (!secondStarted)
          {
            phaseTwoElapsed = 0f;
            secondStarted = true;
          }
          group.alpha = Mathf.Clamp01(1 - phaseTwoElapsed / secondTransitionTime);
          phaseTwoElapsed += Time.deltaTime;
        }
        if (elapsed > firstTransitionTime + secondaryHold + secondTransitionTime)
        {
          UIAnimationEndEvent e = new UIAnimationEndEvent();
          e.name = "colourBackgroundFade";
          EventManager.Broadcast(e);
          animating = false;
        }
      }
      time += Time.deltaTime;
    }
  }
}

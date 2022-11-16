using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFade : MonoBehaviour
{
  public string fadeInAnimationName;
  public string fadeOutAnimationName;
  public float durationIn;
  public float durationOut;
  public float delayIn;
  public float delayOut;
  Image image;
  bool fadingIn;
  bool fadingOut;
  float time;
  private void Awake()
  {
    fadingIn = false;
    fadingOut = false;
    EventManager.AddListener<UIAnimationStartEvent>(onAnimStart);
  }
  private void Start()
  {
    image = GetComponent<Image>();
    Color newColor = image.color;
    newColor.a = 0;
    image.color = newColor;
  }
  private void OnDestroy()
  {
    EventManager.RemoveListener<UIAnimationStartEvent>(onAnimStart);
  }
  void onAnimStart(UIAnimationStartEvent e)
  {
    if (e.name == fadeInAnimationName)
    {
      time = 0f;
      fadingIn = true;
    }
    else if (e.name == fadeOutAnimationName)
    {
      time = 0f;
      fadingOut = true;
    }
  }

  void endFadeIn()
  {
    UIAnimationEndEvent e = new UIAnimationEndEvent();
    e.name = fadeInAnimationName;
    EventManager.Broadcast(e);
    fadingIn = false;
    time = 0f;
  }

  void endFadeOut()
  {
    UIAnimationEndEvent e = new UIAnimationEndEvent();
    e.name = fadeOutAnimationName;
    EventManager.Broadcast(e);
    fadingOut = false;
    time = 0f;
  }

  private void Update()
  {
    if (fadingIn)
    {
      time += Time.deltaTime;
      Color newColor = image.color;
      newColor.a = Mathf.Clamp01((time - delayIn) / durationIn);
      image.color = newColor;
      if (time >= durationIn + delayIn)
      {
        endFadeIn();
      }
    }
    if (fadingOut)
    {
      time += Time.deltaTime;
      Color newColor = image.color;
      newColor.a = Mathf.Clamp01(1 - (time - delayOut) / durationOut);
      image.color = newColor;
      if (time >= durationOut + delayOut)
      {
        endFadeOut();
      }
    }
  }
}

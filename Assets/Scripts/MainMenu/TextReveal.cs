using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextReveal : MonoBehaviour
{
  public string animationName;
  public float duration;
  bool animating;
  float time;
  TextMeshProUGUI mesh;
  string fullText;
  void Awake()
  {
    animating = false;
    time = 0f;
    EventManager.AddListener<UIAnimationStartEvent>(onAnimationStart);
  }
  private void Start()
  {
    mesh = GetComponent<TextMeshProUGUI>();
    fullText = mesh.text;
    mesh.text = "";
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
      float proportion = time / duration;
      int charsToReveal = Mathf.Min(Mathf.RoundToInt(fullText.Length * proportion), fullText.Length);
      mesh.text = fullText.Substring(0, charsToReveal);
      if (time >= duration)
      {
        endAnimation();
      }
    }
  }
}

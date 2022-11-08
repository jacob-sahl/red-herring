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
  string[] fullTextWords;
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
    fullTextWords = mesh.text.Split(' ');
    // Hide the text to start
    mesh.text = "<color=#00000000>" + fullText + "</color>";
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
      string tempText = "<color=#FFFFFFFF>";
      int index = 0;
      while (index < charsToReveal)
      {
        tempText += fullText[index];
        index++;
      }
      tempText += "</color>";
      if (index < fullText.Length)
      {
        tempText += "<color=#FFFFFFBF>";
        tempText += fullText[index];
        tempText += "</color>";
        index++;
      }
      if (index < fullText.Length)
      {
        tempText += "<color=#FFFFFF7F>";
        tempText += fullText[index];
        tempText += "</color>";
        index++;
      }
      if (index < fullText.Length)
      {
        tempText += "<color=#FFFFFF3F>";
        tempText += fullText[index];
        tempText += "</color>";
        index++;
      }
      tempText += "<color=#FFFFFF00>";
      while (index < fullText.Length)
      {
        tempText += fullText[index];
        index++;
      }
      tempText += "</color>";
      mesh.text = tempText;

      // For word-by-word reveal:

      // int wordsToReveal = Mathf.Min(Mathf.RoundToInt(fullTextWords.Length * proportion), fullTextWords.Length);
      // string tempText = "<color=#FFFFFFFF>";
      // int wordIndex = 0;
      // while (wordIndex < wordsToReveal)
      // {
      //   tempText += fullTextWords[wordIndex] + ' ';
      //   wordIndex++;
      // }
      // tempText += "</color>";
      // if (wordIndex < fullTextWords.Length)
      // {
      //   tempText += "<color=#FFFFFFBF>";
      //   tempText += fullTextWords[wordIndex] + ' ';
      //   tempText += "</color>";
      //   wordIndex++;
      // }
      // if (wordIndex < fullTextWords.Length)
      // {
      //   tempText += "<color=#FFFFFF7F>";
      //   tempText += fullTextWords[wordIndex] + ' ';
      //   tempText += "</color>";
      //   wordIndex++;
      // }
      // if (wordIndex < fullTextWords.Length)
      // {
      //   tempText += "<color=#FFFFFF3F>";
      //   tempText += fullTextWords[wordIndex] + ' ';
      //   tempText += "</color>";
      //   wordIndex++;
      // }
      // tempText += "<color=#FFFFFF00>";
      // while (wordIndex < fullTextWords.Length)
      // {
      //   tempText += fullTextWords[wordIndex] + ' ';
      //   wordIndex++;
      // }
      // tempText += "</color>";
      // mesh.text = tempText;

      if (time >= duration)
      {
        endAnimation();
      }
    }
  }
}

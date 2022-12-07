using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextReveal : MonoBehaviour
{
  public string animationName;
  [Tooltip("Number of seconds the animation should take.")]
  public float duration;
  [Tooltip("The color to fade to (alpha is ignored).")]
  public Color targetTextColour;
  [Tooltip("The number of characters to 'fade in' over smoothly.")]
  public int fadeLength = 10;
  public int wordFadeLength = 4;
  bool animating;
  TextWobble wobble;
  float time;
  TextMeshProUGUI mesh;
  string fullText;
  string[] fullTextWords;
  bool hideText;
  int previousStep;
  float[] alphas;
  bool interrupted;
  public bool abortUpdate;

  void Awake()
  {
    animating = false;
    interrupted = false;
    wobble = GetComponent<TextWobble>();
    time = 0f;
    EventManager.AddListener<UIAnimationStartEvent>(onAnimationStart);
    EventManager.AddListener<UIAnimationInterruptAllEvent>(onInterrupt);
  }

  private void OnDestroy()
  {
    EventManager.RemoveListener<UIAnimationStartEvent>(onAnimationStart);
    EventManager.RemoveListener<UIAnimationInterruptAllEvent>(onInterrupt);
  }
  private void Start()
  {
    mesh = GetComponent<TextMeshProUGUI>();
    fullText = mesh.text;
    fullTextWords = mesh.text.Split(' ');
    alphas = new float[mesh.textInfo.characterCount];
    // Hide the text to start
    hideText = true;
    abortUpdate = false;
  }

  private void OnEnable()
  {
    hideText = true;
  }

  void onInterrupt(UIAnimationInterruptAllEvent e)
  {
    if (animating)
    {
      interrupted = true;
    }
  }

  void onAnimationStart(UIAnimationStartEvent e)
  {
    if (e.name == animationName)
    {
      Debug.Log("Starting WITHIN anim. Char count: " + mesh.textInfo.characterCount);
      Debug.Log("Text: " + mesh.text);
      alphas = new float[mesh.textInfo.characterCount];
      time = 0f;
      previousStep = 0;
      animating = true;
      abortUpdate = false;
      if (wobble != null) wobble.setWobbling(false);
    }
  }

  void endAnimation()
  {
    UIAnimationEndEvent e = new UIAnimationEndEvent();
    e.name = animationName;
    EventManager.Broadcast(e);
    animating = false;
    if (wobble != null) wobble.setWobbling(true);
    time = 0f;
  }

  Vector2 Wobble(float time)
  {
    return new Vector2(Mathf.Sin(time * 3.3f), Mathf.Cos(time * 2.5f)) * 0.5f;
  }

  void interruptTextAnimation()
  {
    mesh.ForceMeshUpdate();
    Mesh newMesh = mesh.mesh;
    Color[] colors = newMesh.colors;
    for (int i = 0; i < mesh.textInfo.characterCount; i++)
    {
      TMP_CharacterInfo c = mesh.textInfo.characterInfo[i];

      int index = c.vertexIndex;

      colors[index] = targetTextColour;
      colors[index + 1] = targetTextColour;
      colors[index + 2] = targetTextColour;
      colors[index + 3] = targetTextColour;
    }
    newMesh.colors = colors;
    mesh.canvasRenderer.SetMesh(newMesh);
    interrupted = false;
  }

  void UpdateTextColours(int step)
  {
    mesh.ForceMeshUpdate(); // Necessary to do this before anything else
    Mesh newMesh = mesh.mesh;
    Color[] colors = newMesh.colors;

    int charIndex = 0;
    // mesh.textInfo.wordInfo[0].firstCharacterIndex

    while (charIndex < step && charIndex < mesh.textInfo.characterCount)
    {
      TMP_CharacterInfo c = mesh.textInfo.characterInfo[charIndex];
      // Debug.Log("CharIndex: " + charIndex + " of " + step);

      int index = c.vertexIndex;
      float currentAlpha = alphas[charIndex];
      float targetAlpha = targetTextColour.a;
      // New alpha is 1/fadeLength more than previous, capped at 1
      float newAlpha = Mathf.Min(currentAlpha + targetAlpha * (1f / (float)fadeLength), 1f);
      // Store the new alpha
      alphas[charIndex] = newAlpha;
      // Apply the new alpha to a new color
      Color newColor = new Color(targetTextColour.r, targetTextColour.g, targetTextColour.b, newAlpha);

      colors[index] = newColor;
      colors[index + 1] = newColor;
      colors[index + 2] = newColor;
      colors[index + 3] = newColor;

      charIndex++;
    }

    while (charIndex < mesh.textInfo.characterCount)
    {
      // Make all of the other characters transparent
      TMP_CharacterInfo c = mesh.textInfo.characterInfo[charIndex];

      int index = c.vertexIndex;

      colors[index] = Color.clear;
      colors[index + 1] = Color.clear;
      colors[index + 2] = Color.clear;
      colors[index + 3] = Color.clear;

      charIndex++;
    }

    {
      // First character needs to be reset for some reason
      TMP_CharacterInfo c = mesh.textInfo.characterInfo[0];

      int index = c.vertexIndex;

      float newAlpha = alphas[0];
      Color newColor = new Color(targetTextColour.r, targetTextColour.g, targetTextColour.b, newAlpha);

      colors[index] = newColor;
      colors[index + 1] = newColor;
      colors[index + 2] = newColor;
      colors[index + 3] = newColor;
    }

    newMesh.colors = colors;

    // Wobble:
    Vector3[] vertices = newMesh.vertices;
    for (int i = 0; i < mesh.textInfo.characterCount; i++)
    {
      TMP_CharacterInfo c = mesh.textInfo.characterInfo[i];

      int index = c.vertexIndex;

      Vector3 offset = Wobble(Time.time + i);
      vertices[index] += offset;
      vertices[index + 1] += offset;
      vertices[index + 2] += offset;
      vertices[index + 3] += offset;

    }
    newMesh.vertices = vertices;

    mesh.canvasRenderer.SetMesh(newMesh);
  }

  void Update()
  {
    if (abortUpdate) return;
    // This happens once on the very first Update (NOT Start() because the mesh update doesn't work)
    if (hideText)
    {
      mesh.ForceMeshUpdate();
      Mesh newMesh = mesh.mesh;
      Color[] colors = newMesh.colors;
      for (int i = 0; i < mesh.textInfo.characterCount; i++)
      {
        TMP_CharacterInfo c = mesh.textInfo.characterInfo[i];

        int index = c.vertexIndex;

        colors[index] = Color.clear;
        colors[index + 1] = Color.clear;
        colors[index + 2] = Color.clear;
        colors[index + 3] = Color.clear;
      }
      newMesh.colors = colors;
      mesh.canvasRenderer.SetMesh(newMesh);
      hideText = false;
    }
    else if (animating)
    {
      time += Time.deltaTime;
      float proportion = time / duration;
      int step = Mathf.RoundToInt((mesh.textInfo.characterCount + fadeLength) * proportion);

      if (interrupted)
      {
        interruptTextAnimation();
        endAnimation();
        return;
      }

      if (step > previousStep)
      {
        UpdateTextColours(step);
        previousStep = step;
      }

      if (time >= duration)
      {
        endAnimation();
      }
    }
    else if (wobble != null && wobble.getWobbling())
    {
      // Wobble:
      mesh.ForceMeshUpdate();
      Mesh newMesh = mesh.mesh;
      Vector3[] vertices = newMesh.vertices;
      for (int i = 0; i < mesh.textInfo.characterCount; i++)
      {
        TMP_CharacterInfo c = mesh.textInfo.characterInfo[i];

        int index = c.vertexIndex;

        Vector3 offset = Wobble(Time.time + i);
        vertices[index] += offset;
        vertices[index + 1] += offset;
        vertices[index + 2] += offset;
        vertices[index + 3] += offset;

      }
      newMesh.vertices = vertices;
      mesh.canvasRenderer.SetMesh(newMesh);
    }
  }
}

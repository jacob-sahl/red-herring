using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextHide : MonoBehaviour
{
  public string animationName;
  public float duration;
  string animationInstantName;
  bool animating;
  TextWobble wobble;
  TextMeshProUGUI mesh;
  float time;
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
    wobble = GetComponent<TextWobble>();
    fullText = mesh.text;
    animationInstantName = animationName + "instant";
  }

  private void OnDestroy()
  {
    EventManager.RemoveListener<UIAnimationStartEvent>(onAnimationStart);
  }

  void onAnimationStart(UIAnimationStartEvent e)
  {
    if (e.name == animationName)
    {
      if (wobble != null) wobble.setWobbling(false);
      animating = true;
    }
    else if (e.name == animationInstantName)
    {
      instantHide();
      endInstantAnimation();
    }
  }

  void instantHide()
  {
    mesh.ForceMeshUpdate();
    Mesh newMesh = mesh.mesh;
    Color[] colors = newMesh.colors;
    for (int i = 0; i < mesh.textInfo.characterCount; i++)
    {
      TMP_CharacterInfo c = mesh.textInfo.characterInfo[i];

      int index = c.vertexIndex;
      Color newColor = new Color(colors[0].r, colors[0].g, colors[0].b, 0f);

      colors[index] = newColor;
      colors[index + 1] = newColor;
      colors[index + 2] = newColor;
      colors[index + 3] = newColor;
    }
    newMesh.colors = colors;
    mesh.canvasRenderer.SetMesh(newMesh);
  }

  void endInstantAnimation()
  {
    UIAnimationEndEvent e = new UIAnimationEndEvent();
    e.name = animationInstantName;
    EventManager.Broadcast(e);
  }

  void endAnimation()
  {
    UIAnimationEndEvent e = new UIAnimationEndEvent();
    e.name = animationName;
    EventManager.Broadcast(e);
    animating = false;
    time = 0f;
  }

  Vector2 Wobble(float time)
  {
    return new Vector2(Mathf.Sin(time * 3.3f), Mathf.Cos(time * 2.5f)) * 0.5f;
  }

  void Update()
  {
    if (animating)
    {
      time += Time.deltaTime;
      float proportion = time / duration;
      mesh.ForceMeshUpdate();
      Mesh newMesh = mesh.mesh;
      Color[] colors = newMesh.colors;
      for (int i = 0; i < mesh.textInfo.characterCount; i++)
      {
        TMP_CharacterInfo c = mesh.textInfo.characterInfo[i];

        int index = c.vertexIndex;
        Color newColor = new Color(colors[0].r, colors[0].g, colors[0].b, 1f - proportion);

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
      if (time >= duration)
      {
        endAnimation();
      }
    }
  }
}

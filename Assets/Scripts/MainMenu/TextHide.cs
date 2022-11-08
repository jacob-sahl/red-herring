using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextHide : MonoBehaviour
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
      endAnimation();
    }
  }
}

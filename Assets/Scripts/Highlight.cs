using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
  public Material highlightMaterial;
  public Material transparent;
  Renderer[] renderers;
  bool highlightMaterialsPresent;
  bool highlighted;

  void Start()
  {
    renderers = GetComponentsInChildren<Renderer>();
    highlightMaterialsPresent = false;
    highlighted = false;
  }

  public void showHighlight()
  {
    if (highlighted) return;
    Debug.Log("Showing Highlight on " + gameObject.name);
    foreach (Renderer renderer in renderers)
    {
      Material[] mats = renderer.materials;
      if (highlightMaterialsPresent)
      {
        // Assume the last material is the highlight one, since that's where we put it
        mats[mats.Length - 1] = highlightMaterial;
        renderer.materials = mats;
      }
      else
      {
        Debug.Log("Adding highlight material to " + renderer.gameObject.name);
        List<Material> matList = new List<Material>(mats);
        matList.Add(highlightMaterial);
        renderer.materials = matList.ToArray();
      }
    }
    highlightMaterialsPresent = true;
    highlighted = true;
  }

  public void hideHighlight()
  {
    if (!highlighted) return;
    Debug.Log("Hiding Highlight on " + gameObject.name);
    foreach (Renderer renderer in renderers)
    {
      Debug.Log("Turning highlight material transparent on " + renderer.gameObject.name);
      Material[] mats = renderer.materials;
      mats[mats.Length - 1] = transparent;
      renderer.materials = mats;
    }
    highlighted = false;
  }
}

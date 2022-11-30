using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Chandelier : MonoBehaviour
{
  HDAdditionalLightData[] lights;
  public float onIntensity = 600f;
  public float offIntensity = 0f;
  bool illuminated;
  bool dimming;
  bool illuminating;
  float time;
  public GameObject lightManagerObj;
  Lights lightManager;
  public float transitionDuration = 0.25f;
  public List<GameObject> flames;

  void Start()
  {
    lights = GetComponentsInChildren<HDAdditionalLightData>();
    illuminated = true;
    dimming = false;
    illuminating = false;
    time = 0f;
    lightManager = lightManagerObj.GetComponent<Lights>();
    lightManager.registerLightState(gameObject, true);
  }

  private void Awake()
  {
    EventManager.AddListener<InteractEvent>(onInteract);
  }

  private void OnDestroy()
  {
    EventManager.RemoveListener<InteractEvent>(onInteract);
  }

  void onInteract(InteractEvent evt)
  {
    if (evt.gameObject == gameObject)
    {
      if (dimming || illuminating) return;
      if (illuminated)
      {
        turnOffLights();
      }
      else
      {
        turnOnLights();
      }

    }
  }

  void turnOffLights()
  {
    time = 0f;
    dimming = true;
    foreach (GameObject flame in flames)
    {
      flame.GetComponent<Renderer>().enabled = false;
    }
    lightManager.updateLightState(gameObject, false);
  }

  void turnOnLights()
  {
    time = 0f;
    illuminating = true;
    foreach (GameObject flame in flames)
    {
      flame.GetComponent<Renderer>().enabled = true;
    }
    lightManager.updateLightState(gameObject, true);
  }

  void updateLights(float intensity)
  {
    foreach (HDAdditionalLightData light in lights)
    {
      light.intensity = intensity;
    }
  }

  // Update is called once per frame
  void Update()
  {
    if (dimming)
    {
      time += Time.deltaTime;
      float propotionalChange = Mathf.Min(time / transitionDuration, 1);
      updateLights(onIntensity - (onIntensity - offIntensity) * propotionalChange);
      if (time > transitionDuration)
      {
        dimming = false;
        illuminated = false;
      }
    }
    if (illuminating)
    {
      time += Time.deltaTime;
      float propotionalChange = Mathf.Min(time / transitionDuration, 1);
      updateLights(offIntensity + (onIntensity - offIntensity) * propotionalChange);
      if (time > transitionDuration)
      {
        illuminating = false;
        illuminated = true;
      }
    }
  }
}

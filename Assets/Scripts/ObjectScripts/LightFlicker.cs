using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(HDAdditionalLightData))]
public class LightFlicker : MonoBehaviour
{
  HDAdditionalLightData lightData;
  public float flickerInterval;
  public float minIntensity;
  public float maxIntensity;
  float time;
  // Start is called before the first frame update
  void Start()
  {
    lightData = GetComponent<HDAdditionalLightData>();
    time = 0f;
  }
  private void Update()
  {
    time += Time.deltaTime;
    if (time >= flickerInterval)
    {
      lightData.intensity = Random.Range(minIntensity, maxIntensity);
      time = 0f;
    }

  }
}

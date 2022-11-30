using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lights : MonoBehaviour
{
  Dictionary<GameObject, bool> LightStates;
  List<SecretObjectiveID> broadcasted;

  private void Awake()
  {
    LightStates = new Dictionary<GameObject, bool>();
    broadcasted = new List<SecretObjectiveID>();
  }

  public void registerLightState(GameObject obj, bool state)
  {
    LightStates.Add(obj, state);
  }

  public void updateLightState(GameObject obj, bool state)
  {
    LightStates[obj] = state;
    if (checkAllLightsOff())
    {
      //   Debug.Log("All lights off!");
      if (broadcasted.Contains(SecretObjectiveID.Blackout)) return;
      SecretObjectiveEvent evt = new SecretObjectiveEvent();
      evt.id = SecretObjectiveID.Blackout;
      evt.status = true;
      EventManager.Broadcast(evt);
    }
  }

  bool checkAllLightsOff()
  {
    foreach (bool state in LightStates.Values)
    {
      if (state) return false;
    }
    return true;
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
  private bool looking;
  private float timeLooked;
  private bool broadcasted = false;
  void Awake()
  {
    EventManager.AddListener<LookEvent>(onLook);
    timeLooked = 0f;
  }

  private void OnDestroy()
  {
    EventManager.RemoveListener<LookEvent>(onLook);
  }

  void onLook(LookEvent evt)
  {
    if (evt.gameObject == gameObject)
    {
      looking = true;
    }
    else
    {
      // Reset clock
      timeLooked = 0f;
      looking = false;
    }
  }

  void Update()
  {
    if (looking)
    {
      timeLooked += Time.deltaTime;
      // If the player has been looking for 3 consecutive seconds
      if (timeLooked >= 3f && !broadcasted)
      {
        SecretObjectiveEvent soEvt = new SecretObjectiveEvent();
        soEvt.id = SecretObjectiveID.LookThroughWindow;
        soEvt.status = true;
        EventManager.Broadcast(soEvt);
        broadcasted = true;
      }
    }
  }
}

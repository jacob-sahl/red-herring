using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeWriter : MonoBehaviour
{
  private bool broadcasted = false;
  void Update()
  {
    if (Vector3.Dot(transform.up, Vector3.down) > 0 && !broadcasted)
    {
      SecretObjectiveEvent evt = new SecretObjectiveEvent();
      evt.id = SecretObjectiveID.InvertTypewriter;
      evt.status = true;
      EventManager.Broadcast(evt);
      broadcasted = true;
    }
  }
}

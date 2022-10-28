using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : MonoBehaviour
{
  private bool broadcastedOffShelf = false;
  private Vector3 initPos;
  void Start()
  {
    initPos = transform.position;
  }

  void Update()
  {
    if (initPos.y - transform.position.y > 1 && !broadcastedOffShelf)
    {
      SecretObjectiveEvent e = new SecretObjectiveEvent();
      e.id = SecretObjectiveID.SkullOffShelf;
      e.status = true;
      EventManager.Broadcast(e);
      broadcastedOffShelf = true;
    }
  }
}

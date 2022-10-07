using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Focus : MonoBehaviour
{
  private Rigidbody rb;
  void Start()
  {
    EventManager.AddListener<FocusEvent>(OnFocus);
    rb = GetComponent<Rigidbody>();
  }

  public void OnFocus(FocusEvent evt)
  {
    disablePhysics();
  }

  public void disablePhysics()
  {
    rb.isKinematic = true;
  }

  public void enablePhysics()
  {
    rb.isKinematic = false;
  }
}

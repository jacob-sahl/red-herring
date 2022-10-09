using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Focus : MonoBehaviour
{
  [Tooltip("The rotation that this object will start out with when inspected.")]
  public Vector3 defaultRotation;
  public float focusDistance;
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

  public void disableCollider()
  {
    GetComponent<BoxCollider>().enabled = false;
  }

  public void enableCollider()
  {
    GetComponent<BoxCollider>().enabled = true;
  }

  public void disablePhysics()
  {
    rb.isKinematic = true;
    disableCollider();
  }

  public void enablePhysics()
  {
    rb.isKinematic = false;
    enableCollider();
  }
}

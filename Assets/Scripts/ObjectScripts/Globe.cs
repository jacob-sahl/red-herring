using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globe : MonoBehaviour
{
  private int n_spins;
  private Animator _anim;
  private bool spinsBroadcasted = false;
  void Awake()
  {
    EventManager.AddListener<InteractEvent>(onInteract);
    n_spins = 0;
  }
  private void OnDestroy()
  {
    EventManager.RemoveListener<InteractEvent>(onInteract);
  }

  void Start()
  {
    _anim = GetComponent<Animator>();
  }

  void onInteract(InteractEvent e)
  {
    if (e.gameObject == gameObject)
    {
      spin();
    }
  }

  void spin()
  {
    _anim.SetTrigger("SpinOnce");
    n_spins++;
    if (n_spins >= 3 && !spinsBroadcasted)
    {
      SecretObjectiveEvent e = new SecretObjectiveEvent();
      e.id = SecretObjectiveID.SpinGlobeThrice;
      e.status = true;
      EventManager.Broadcast(e);
      spinsBroadcasted = true;
    }
  }

}

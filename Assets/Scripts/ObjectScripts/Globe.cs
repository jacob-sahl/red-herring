using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Globe : MonoBehaviour
{
  private Animator _anim;
  private int n_spins;
  private bool spinsBroadcasted;
  public AudioClip spinSound;
  AudioSource audioSource;

  private void Awake()
  {
    EventManager.AddListener<InteractEvent>(onInteract);
    n_spins = 0;
  }

  private void Start()
  {
    _anim = GetComponent<Animator>();
    audioSource = GetComponent<AudioSource>();
  }

  private void OnDestroy()
  {
    EventManager.RemoveListener<InteractEvent>(onInteract);
  }

  private void onInteract(InteractEvent e)
  {
    if (e.gameObject == gameObject) spin();
  }

  private void spin()
  {
    audioSource.PlayOneShot(spinSound);
    _anim.SetTrigger("SpinOnce");
    n_spins++;
    if (n_spins >= 3 && !spinsBroadcasted)
    {
      var e = new SecretObjectiveEvent();
      e.id = SecretObjectiveID.SpinGlobeThrice;
      e.status = true;
      EventManager.Broadcast(e);
      spinsBroadcasted = true;
    }
  }
}
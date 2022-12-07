using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Focus : MonoBehaviour
{
  public AudioClip pickupSound;
  [Tooltip("The rotation that this object will start out with when inspected.")]
  public Vector3 defaultRotation;
  public Vector3 defaultTranslation;
  public float focusDistance;
  private Rigidbody rb;
  public float lightLevel = 0;
  AudioSource audioSource;

  private void Start()
  {
    EventManager.AddListener<FocusEvent>(OnFocus);
    rb = GetComponent<Rigidbody>();
    audioSource = GetComponent<AudioSource>();
  }

  private void OnDestroy()
  {
    EventManager.RemoveListener<FocusEvent>(OnFocus);
  }

  public void OnFocus(FocusEvent evt)
  {
    if (evt.gameObject == gameObject)
    {
      disablePhysics();
      if (audioSource == null || pickupSound == null) return;
      audioSource.PlayOneShot(pickupSound);
    }
  }

  public void disableCollider()
  {
    GetComponent<Collider>().enabled = false;
  }

  public void enableCollider()
  {
    GetComponent<Collider>().enabled = true;
  }

  public void disablePhysics()
  {
    if (rb != null) rb.isKinematic = true;
    disableCollider();
  }

  public void enablePhysics()
  {
    if (rb != null) rb.isKinematic = false;
    enableCollider();
  }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskDrawer : MonoBehaviour
{
  static int drawersOpened;
  public float transitionDuration;
  float time;
  bool ajar;
  bool moving;
  public float slideDistance;
  bool opened;


  // Start is called before the first frame update
  void Start()
  {
    time = 0f; ajar = false; moving = false; opened = false; drawersOpened = 0;
  }

  private void Awake()
  {
    EventManager.AddListener<InteractEvent>(onInteract);
  }

  private void OnDestroy()
  {
    EventManager.RemoveListener<InteractEvent>(onInteract);
  }

  void onInteract(InteractEvent evt)
  {
    if (evt.gameObject == gameObject)
    {
      if (!opened)
      {
        opened = true;
        drawersOpened++;
        if (drawersOpened == 9)
        {
          updateDrawersOpenSO();
        }
      }
      if (moving) return;
      if (ajar)
      {
        StartCoroutine(move(transform.position + new Vector3(0, 0, slideDistance)));
        ajar = false;
      }
      else
      {
        StartCoroutine(move(transform.position - new Vector3(0, 0, slideDistance)));
        ajar = true;
      }
    }
  }

  IEnumerator move(Vector3 target)
  {
    moving = true;
    Vector3 startingPosition = transform.position;
    time = 0f;
    while (time < transitionDuration)
    {
      float propotionalChange = Mathf.Min(time / transitionDuration, 1);
      time += Time.deltaTime;
      transform.position = Vector3.Lerp(startingPosition, target, propotionalChange);
      yield return new WaitForEndOfFrame();
    }
    moving = false;
  }

  void updateDrawersOpenSO()
  {
    SecretObjectiveEvent evt = new SecretObjectiveEvent();
    evt.id = SecretObjectiveID.OpenDeskDrawers;
    evt.status = true;
    EventManager.Broadcast(evt);
  }
}

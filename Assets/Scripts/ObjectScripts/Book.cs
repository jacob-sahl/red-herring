using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
  public static int booksPickedUp;
  bool pickedUp;
  void Start()
  {
    booksPickedUp = 0;
    pickedUp = false;
  }

  private void Awake()
  {
    EventManager.AddListener<FocusEvent>(onFocus);
  }

  private void OnDestroy()
  {
    EventManager.RemoveListener<FocusEvent>(onFocus);
  }

  void onFocus(FocusEvent evt)
  {
    if (evt.gameObject != gameObject) return;
    if (pickedUp) return;
    booksPickedUp++;
    // Debug.Log("Books picked up: " + booksPickedUp);
    pickedUp = true;
    if (booksPickedUp == 10)
    {
      SecretObjectiveEvent e = new SecretObjectiveEvent();
      e.id = SecretObjectiveID.Librarian;
      e.status = true;
      EventManager.Broadcast(e);
    }
  }
}

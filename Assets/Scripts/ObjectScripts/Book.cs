using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
  public static int booksPickedUp;
  bool pickedUp;
  float inspectionTime;
  bool beingInspected;
  List<SecretObjectiveID> broadcasted;
  void Start()
  {
    booksPickedUp = 0; pickedUp = false; inspectionTime = 0f; beingInspected = false;
    broadcasted = new List<SecretObjectiveID>();
  }

  private void Awake()
  {
    EventManager.AddListener<FocusEvent>(onFocus);
    EventManager.AddListener<DefocusEvent>(onDefocus);
  }

  private void OnDestroy()
  {
    EventManager.RemoveListener<FocusEvent>(onFocus);
    EventManager.RemoveListener<DefocusEvent>(onDefocus);
  }

  void onFocus(FocusEvent evt)
  {
    if (evt.gameObject != gameObject) return;
    if (!pickedUp)
    {
      booksPickedUp++;
      pickedUp = true;
      if (booksPickedUp == 10)
      {
        SecretObjectiveEvent e = new SecretObjectiveEvent();
        e.id = SecretObjectiveID.Librarian;
        e.status = true;
        EventManager.Broadcast(e);
      }
    }
    beingInspected = true;
  }

  void onDefocus(DefocusEvent evt)
  {
    if (evt.gameObject != gameObject) return;
    beingInspected = false;
    inspectionTime = 0f;
  }

  void sendBookInspectionSO()
  {
    if (broadcasted.Contains(SecretObjectiveID.CarefulBookInspection)) return;
    SecretObjectiveEvent evt = new SecretObjectiveEvent();
    evt.id = SecretObjectiveID.CarefulBookInspection;
    evt.status = true;
    EventManager.Broadcast(evt);
    broadcasted.Add(SecretObjectiveID.CarefulBookInspection);
  }

  private void Update()
  {
    if (beingInspected)
    {
      inspectionTime += Time.deltaTime;
      if (inspectionTime >= 10f)
      {
        sendBookInspectionSO();
      }
    }
  }
}

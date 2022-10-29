using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
  GameObject handCentre;
  GameObject minuteHand;
  GameObject hourHand;
  GameObject clockHead;
  public float rotationY = 12.765f;
  public GameObject debugSpherePrefab;
  public int clockTimeMinutes;
  Vector3 pivot;
  Vector3 axis;
  void Awake()
  {
    EventManager.AddListener<InteractEvent>(onInteract);
  }
  void Start()
  {
    clockTimeMinutes = 180;
    handCentre = transform.Find("head/handCentre").gameObject;
    minuteHand = transform.Find("head/minuteHand").gameObject;
    hourHand = transform.Find("head/hourHand").gameObject;
    clockHead = transform.Find("head").gameObject;
    Debug.Log("HEAD: " + clockHead.name);
    setUpRotation();
  }

  void setUpRotation()
  {
    pivot = handCentre.transform.position;
    pivot += new Vector3(0, rotationY, 0);
    // GameObject sphere = Instantiate(debugSpherePrefab);
    // sphere.transform.position = pivot;
    // sphere.name = "DEBUG SPHERE";
    axis = handCentre.transform.up;
  }

  void onInteract(InteractEvent e)
  {
    if (e.gameObject == clockHead)
    {
      rotateHands();
    }
  }

  void rotateHands()
  {
    minuteHand.transform.RotateAround(pivot, axis, -30f);
    hourHand.transform.RotateAround(pivot, axis, -2.5f);
    clockTimeMinutes += 5;
    if (clockTimeMinutes >= 720)
    {
      clockTimeMinutes = 0;
    }
  }
  // void rotateHourHand()
  // {
  //   hourHand.transform.RotateAround(pivot, axis, 30);
  // }
}

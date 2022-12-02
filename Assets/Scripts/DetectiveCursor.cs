using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CursorType
{
  Hand,
  Inspection,
  Default
}

public class DetectiveCursor : MonoBehaviour
{
  public GameObject defaultCursor;
  public GameObject inspectionCursor;
  public GameObject handCursors;
  public float expandTime = 0.2f;
  GameObject handCursor;
  CursorType showing;
  bool focused;
  private void Awake()
  {
    EventManager.AddListener<FocusEvent>(onFocus);
    EventManager.AddListener<DefocusEvent>(onDefocus);
    EventManager.AddListener<LookEvent>(onLook);
  }

  private void OnDestroy()
  {
    EventManager.RemoveListener<FocusEvent>(onFocus);
    EventManager.RemoveListener<DefocusEvent>(onDefocus);
    EventManager.RemoveListener<LookEvent>(onLook);
  }
  // Start is called before the first frame update
  void Start()
  {
    //selectRandomHandCursor();
    handCursor = handCursors;
    showDefaultCursor();
    focused = false;
  }

  void onFocus(FocusEvent evt)
  {
    focused = true;
    showHandCursor();
  }

  void onDefocus(DefocusEvent evt)
  {
    focused = false;
    showDefaultCursor();
  }

  void onLook(LookEvent evt)
  {
    if (focused) return;
    if (evt.gameObject.GetComponent<Focus>() != null)
    {
      if (showing != CursorType.Inspection)
      {
        showInspectionCursor();
      }

    }
    else if (evt.gameObject.GetComponent<Interactable>() != null)
    {
      if (showing != CursorType.Hand)
      {
        showHandCursor();
      }
    }
    else
    {
      if (showing != CursorType.Default)
      {
        showDefaultCursor();
      }
    }

  }

  void selectRandomHandCursor()
  {
    int target = Mathf.FloorToInt(Random.Range(0, 5));
    int i = 0;
    foreach (Transform child in handCursors.transform)
    {
      if (i == target)
      {
        handCursor = child.gameObject;
      }
      else
      {
        child.gameObject.SetActive(false);
      }
      i++;
    }
  }

  void showHandCursor()
  {
    inspectionCursor.SetActive(false);
    defaultCursor.SetActive(false);
    handCursor.SetActive(true);
    showing = CursorType.Hand;
    StopCoroutine("ExpandCursor");
    StartCoroutine(ExpandCursor());
  }

  void showInspectionCursor()
  {
    inspectionCursor.SetActive(true);
    defaultCursor.SetActive(false);
    handCursor.SetActive(false);
    showing = CursorType.Inspection;
    StopCoroutine("ExpandCursor");
    StartCoroutine(ExpandCursor());
  }

  void showDefaultCursor()
  {
    inspectionCursor.SetActive(false);
    defaultCursor.SetActive(true);
    handCursor.SetActive(false);
    showing = CursorType.Default;
    StopCoroutine("ExpandCursor");
    transform.localScale = Vector3.one;
  }

  private IEnumerator ExpandCursor()
  {
    float timeElapsed = 0;
    while (timeElapsed < expandTime)
    {
      transform.localScale = Vector3.one * Mathf.Lerp(0, 1, timeElapsed / expandTime);
      timeElapsed += Time.deltaTime;
      yield return null;
    }
    transform.localScale = Vector3.one;
  }
}

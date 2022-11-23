using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeWriter : MonoBehaviour
{
  public AudioClip keydownClip;
  private AudioSource audioSource;
  private TypeWriterPuzzleID activePuzzle;
  private TypeWriterPuzzle _typeWriterPuzzle;
  private List<SecretObjectiveID> broadcasted = new List<SecretObjectiveID>();
  // private GameObject carriageGroup;
  private Animator animator;
  float time;
  void Awake()
  {
    audioSource = gameObject.GetComponent<AudioSource>();
    EventManager.AddListener<DefocusEvent>(onDefocus);
    // carriageGroup = transform.Find("CarriageGroup").gameObject;
  }
  private void OnDestroy()
  {
    EventManager.RemoveListener<DefocusEvent>(onDefocus);
  }
  void Start()
  {
    time = 0f;
    animator = GetComponentInChildren<Animator>();
    activePuzzle = GameController.Instance.getCurrentPuzzle().id;
    _typeWriterPuzzle = gameObject.GetComponent<TypeWriterPuzzle>();
    switch (activePuzzle)
    {
      case TypeWriterPuzzleID.BlueRedYellow:
        colorStrikers();
        break;
    }
    // rightSetCarriage();
  }
  // void rightSetCarriage()
  // {
  //   carriageGroup.transform.Translate(new Vector3(-1.5f, 0f, 0f));
  // }
  // void bumpCarriageLeft() {

  // }
  void Update()
  {
    time += Time.deltaTime;
    if (Vector3.Dot(transform.up, Vector3.down) > 0 && !broadcasted.Contains(SecretObjectiveID.InvertTypewriter))
    {
      SecretObjectiveEvent evt = new SecretObjectiveEvent();
      evt.id = SecretObjectiveID.InvertTypewriter;
      evt.status = true;
      EventManager.Broadcast(evt);
      broadcasted.Add(evt.id);
    }
  }
  void onDefocus(DefocusEvent evt)
  {
    if (evt.gameObject == gameObject)
    {
      if (_typeWriterPuzzle.CheckAnswer() && !broadcasted.Contains(SecretObjectiveID.DropCorrect))
      {
        SecretObjectiveEvent e = new SecretObjectiveEvent();
        e.id = SecretObjectiveID.DropCorrect;
        e.status = true;
        EventManager.Broadcast(e);
        broadcasted.Add(e.id);
      }
    }
  }
  public void moveCarriageGroup()
  {

  }
  public void playKeydownClip()
  {
    audioSource.PlayOneShot(keydownClip);
  }
  void colorStrikers()
  {
    GameObject strikerParent = transform.Find("PR_Strikers_low").gameObject;
    MeshRenderer[] meshes = strikerParent.GetComponentsInChildren<MeshRenderer>();
    for (int i = 0; i < meshes.Length; i++)
    {
      if (i % 3 == 0)
      {
        meshes[i].material.color = Color.blue;
      }
      else if (i % 3 == 1)
      {
        meshes[i].material.color = Color.red;
      }
      else
      {
        meshes[i].material.color = Color.yellow;
      }
    }
  }
}

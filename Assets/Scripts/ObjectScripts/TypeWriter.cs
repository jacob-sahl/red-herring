using System.Collections.Generic;
using UnityEngine;

public class TypeWriter : MonoBehaviour
{
  public AudioClip keydownClip;
  private TypeWriterPuzzle _typeWriterPuzzle;
  private TypeWriterPuzzleID activePuzzle;
  private AudioSource audioSource;
  private readonly List<SecretObjectiveID> broadcasted = new();
  private GameObject carriageGroup;

  private void Awake()
  {
    audioSource = gameObject.GetComponent<AudioSource>();
    EventManager.AddListener<DefocusEvent>(onDefocus);
    carriageGroup = transform.Find("CarriageGroup").gameObject;
  }

  private void Start()
  {
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
  private void Update()
  {
    if (Vector3.Dot(transform.up, Vector3.down) > 0 && !broadcasted.Contains(SecretObjectiveID.InvertTypewriter))
    {
      var evt = new SecretObjectiveEvent();
      evt.id = SecretObjectiveID.InvertTypewriter;
      evt.status = true;
      EventManager.Broadcast(evt);
      broadcasted.Add(evt.id);
    }
  }

  private void OnDestroy()
  {
    EventManager.RemoveListener<DefocusEvent>(onDefocus);
  }

  private void onDefocus(DefocusEvent evt)
  {
    if (evt.gameObject == gameObject)
      if (_typeWriterPuzzle.CheckAnswer() && !broadcasted.Contains(SecretObjectiveID.DropCorrect))
      {
        var e = new SecretObjectiveEvent();
        e.id = SecretObjectiveID.DropCorrect;
        e.status = true;
        EventManager.Broadcast(e);
        broadcasted.Add(e.id);
      }
  }

  public void moveCarriageGroup()
  {
  }

  public void playKeydownClip()
  {
    audioSource.PlayOneShot(keydownClip);
  }

  private void colorStrikers()
  {
    var strikerParent = transform.Find("PR_Strikers_low").gameObject;
    var meshes = strikerParent.GetComponentsInChildren<MeshRenderer>();
    for (var i = 0; i < meshes.Length; i++)
      if (i % 3 == 0)
        meshes[i].material.color = Color.blue;
      else if (i % 3 == 1)
        meshes[i].material.color = Color.red;
      else
        meshes[i].material.color = Color.yellow;
  }
}
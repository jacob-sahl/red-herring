using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeWriter : MonoBehaviour
{
  public AudioClip keydownClip;
  private bool broadcasted = false;
  private AudioSource audioSource;
  private TypeWriterPuzzleID activePuzzle;
  void Awake()
  {
    audioSource = gameObject.GetComponent<AudioSource>();
  }
  void Start()
  {
    activePuzzle = GameController.Instance.getCurrentPuzzle().id;
    switch (activePuzzle)
    {
      case TypeWriterPuzzleID.BlueRedYellow:
        colorStrikers();
        break;
    }
  }
  void Update()
  {
    if (Vector3.Dot(transform.up, Vector3.down) > 0 && !broadcasted)
    {
      SecretObjectiveEvent evt = new SecretObjectiveEvent();
      evt.id = SecretObjectiveID.InvertTypewriter;
      evt.status = true;
      EventManager.Broadcast(evt);
      broadcasted = true;
    }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintPaper : MonoBehaviour
{
  [Tooltip("The puzzle(s) this object should appear within.")]
  public List<TypeWriterPuzzleID> puzzles;
  void Start()
  {
    TypeWriterPuzzleID activePuzzle = GameController.Instance.getCurrentPuzzle().id;
    if (!puzzles.Contains(activePuzzle))
    {
      Debug.Log("Hiding hint paper");
      gameObject.SetActive(false);
    }
  }
}

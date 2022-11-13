using System.Collections.Generic;
using UnityEngine;

public class HintPaper : MonoBehaviour
{
    [Tooltip("The puzzle(s) this object should appear within.")]
    public List<TypeWriterPuzzleID> puzzles;

    private void Start()
    {
        var activePuzzle = GameController.Instance.getCurrentPuzzle().id;
        if (!puzzles.Contains(activePuzzle)) gameObject.SetActive(false);
    }
}
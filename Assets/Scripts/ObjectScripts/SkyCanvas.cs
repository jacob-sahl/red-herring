using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SkyCanvas : MonoBehaviour
{
  TextMeshProUGUI text;
  TypeWriterPuzzleInstance puzzle;
  void Start()
  {
    text = transform.Find("SkyText").gameObject.GetComponent<TextMeshProUGUI>();
    puzzle = GameController.Instance.getCurrentPuzzle();
    switch (puzzle.id)
    {
      case TypeWriterPuzzleID.PlantsAndAnimals:
        text.text = "PLANTS AND ANIMALS";
        break;
      default:
        text.text = "";
        break;
    }
  }

}

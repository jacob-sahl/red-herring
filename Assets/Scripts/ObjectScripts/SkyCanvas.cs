using TMPro;
using UnityEngine;

public class SkyCanvas : MonoBehaviour
{
    private TypeWriterPuzzleInstance puzzle;
    private TextMeshProUGUI text;

    private void Start()
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
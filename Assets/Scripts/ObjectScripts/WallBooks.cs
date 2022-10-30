using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WallBooks : MonoBehaviour
{
  private Dictionary<TypeWriterPuzzleID, string[]> PuzzleToBookTitles = new Dictionary<TypeWriterPuzzleID, string[]>()
  {
    {TypeWriterPuzzleID.BlueRedYellow, new string[] {
      "Hey, this is the First Puzzle!",
      "Look Alikes & Dopplegangers Among Us",
      "At Giovanni's",
      "The Curse of Erring Manor",
      "Colours and Sounds",
      "Of Ghosts and Ghouls",
      "The Other Curse of Erring Manor",
      "Left Behind",
      "Bookshelf Arrangement Ettiquette",
      "!"
    }},
    {TypeWriterPuzzleID.One2Three, new string[] {
      "This is the Second Puzzle",
      "",
      "",
      "",
      "",
      "",
      "",
      "",
      "",
      ""
    }},
    {TypeWriterPuzzleID.FearOfElephants, new string[] {
      "Book #1",
      "How To Make Your Manor a Home",
      "Cryptozoology No. 19",
      "An Introduction to Pachydermophobia",
      "The Last One Seems Important",
      "Der Katzenprinz",
      "Poisons Most Foul",
      "The World's Longest Necks",
      "Modern Wallpaper, Summer Issue 18XX",
      "SCARY DUMBO: An Anachronistic Horror Collecction"
    }},
    {TypeWriterPuzzleID.PlantsAndAnimals, new string[] {
      "This is the Fourth Puzzle",
      "",
      "",
      "",
      "",
      "",
      "",
      "",
      "",
      ""
    }},
  };
  void Start()
  {
    TypeWriterPuzzleInstance puzzle = GameController.Instance.getCurrentPuzzle();
    applyBookTitles(puzzle.id);
  }
  void applyBookTitles(TypeWriterPuzzleID puzzleID)
  {
    string[] titles = PuzzleToBookTitles[puzzleID];
    for (int i = 1; i < 11; i++)
    {
      Debug.Log("Looking For: " + "wallBooksCanvas/bookText" + i.ToString());
      GameObject text = transform.Find("wallBooksCanvas/bookText" + i.ToString()).gameObject;
      text.GetComponent<TextMeshPro>().text = titles[i - 1];
    }
  }
}

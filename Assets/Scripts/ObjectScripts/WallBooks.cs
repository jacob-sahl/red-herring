using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WallBooks : MonoBehaviour
{
  private Dictionary<TypeWriterPuzzleID, string[]> PuzzleToBookTitles = new Dictionary<TypeWriterPuzzleID, string[]>()
  {
    {TypeWriterPuzzleID.BlueRedYellow, new string[] {
      "The First Room",
      "Solution, Mixtures & Cures",
      "Is the Mystery Apparent?",
      "The Curse of Erring Manor",
      "Colours and Sounds",
      "Of Ghosts and Ghouls",
      "The Other Curse of Erring Manor",
      "Left Behind",
      "Bookshelf Arrangement Ettiquette",
      "!"
    }},
    {TypeWriterPuzzleID.One2Three, new string[] {
      "T",
      "I",
      "C",
      "K",
      "T",
      "O",
      "C",
      "K",
      "Serial No. 123?",
      "Serial No. OneTwoThree?"
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
      "Wisteria & Panthera leo",
      "Juniperus virginiana & Bos gaurus",
      "Digitalis & Vulpes vulpes",
      "Hydrangea & Crocodylus palustris",
      "Syringa & Equus caballus",
      "Acer & Canis lupus",
      "Quercus & Camelus dromedaries",
      "Pyrus & Giraffa camelopardalis",
      "Hibiscus & Elephas maximus",
      "Helleborus & Felis domesticus"
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
      GameObject text = transform.Find("wallBooksCanvas/bookText" + i.ToString()).gameObject;
      text.GetComponent<TextMeshPro>().text = titles[i - 1];
    }
  }
}

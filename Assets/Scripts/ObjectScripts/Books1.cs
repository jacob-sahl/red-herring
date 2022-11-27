using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Books1 : MonoBehaviour
{
  private readonly Dictionary<TypeWriterPuzzleID, string[]> PuzzleToBookCovers = new()
  {
    {
      TypeWriterPuzzleID.BlueRedYellow, new[]
      {
        "The Vampire",
        "Solution, Mixtures & Cures",
        "Is the Mystery Apparent?",
        "These Curse of Erring Manor",
        "Words: Colours and Sounds",
        "Blue Ghosts: How to Tell if Your Spectre is Depressed",
        "Red Rum",
        "Yellow Jacket Survival Guide",
        "!",
        "",
        "",
        "",
        "Bookshelf Arrangement Ettiquette: The First Words are Vital",
      }
    },
    {
      TypeWriterPuzzleID.One2Three, new[]
      {
        "T",
        "I",
        "C",
        "K",
        "T",
        "O",
        "C",
        "K",
        "Serial No. 123?",
        "Serial No. OneTwoThree?",
        "",
        "",
        "",
      }
    },
    {
      TypeWriterPuzzleID.FearOfElephants, new[]
      {
        "Book #1",
        "How To Make Your Manor a Home",
        "Cryptozoology No. 19",
        "An Introduction to PACHYDERMOPHOBIA",
        "The Last One Seems Important",
        "Der Katzenprinz",
        "Poisons Most Foul",
        "The World's Longest Necks",
        "Modern Wallpaper, Summer Issue 18XX",
        "SCARY DUMBO: An Anachronistic Horror Collection",
        "",
        "",
        "",
      }
    },
    {
      TypeWriterPuzzleID.PlantsAndAnimals, new[]
      {
        "Wisteria & Panthera leo",
        "Juniperus virginiana & Bos gaurus",
        "Digitalis & Vulpes vulpes",
        "Hydrangea & Crocodylus palustris",
        "Syringa & Equus caballus",
        "Acer & Canis lupus",
        "Quercus & Camelus dromedaries",
        "Pyrus & Giraffa camelopardalis",
        "Hibiscus & Elephas maximus",
        "Helleborus & Felis domesticus",
        "",
        "",
        "",
      }
    }
  };

  private string[] standardSpines = { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII" };
  private readonly Dictionary<TypeWriterPuzzleID, string[]> PuzzleToBookSpines = new()
  {
  };

  private void Start()
  {
    var puzzle = GameController.Instance.getCurrentPuzzle();
    applyBookTitles(puzzle.id);
    // applyBookSpines(puzzle.id);
  }

  private void applyBookTitles(TypeWriterPuzzleID puzzleID)
  {
    var titles = PuzzleToBookCovers[puzzleID];
    for (var i = 1; i < 14; i++)
    {
      GameObject text = transform.Find("Book1-" + i + "/BookCanvas/CoverText").gameObject;
      text.GetComponent<TextMeshProUGUI>().text = titles[i - 1];
    }
  }

  private void applyBookSpines(TypeWriterPuzzleID puzzleID)
  {
    for (var i = 1; i < 14; i++)
    {
      GameObject text = transform.Find("Book1-" + i + "/BookCanvas/SpineText").gameObject;
      text.GetComponent<TextMeshProUGUI>().text = standardSpines[i - 1];
    }
  }
}
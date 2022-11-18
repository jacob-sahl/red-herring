using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WallBooks : MonoBehaviour
{
    private readonly Dictionary<TypeWriterPuzzleID, string[]> PuzzleToBookTitles = new()
    {
        {
            TypeWriterPuzzleID.BlueRedYellow, new[]
            {
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
                "Serial No. OneTwoThree?"
            }
        },
        {
            TypeWriterPuzzleID.FearOfElephants, new[]
            {
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
                "Helleborus & Felis domesticus"
            }
        }
    };

    private void Start()
    {
        var puzzle = GameController.Instance.getCurrentPuzzle();
        applyBookTitles(puzzle.id);
    }

    private void applyBookTitles(TypeWriterPuzzleID puzzleID)
    {
        var titles = PuzzleToBookTitles[puzzleID];
        for (var i = 1; i < 11; i++)
        {
            var text = transform.Find("wallBooksCanvas/bookText" + i).gameObject;
            text.GetComponent<TextMeshPro>().text = titles[i - 1];
        }
    }
}
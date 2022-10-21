using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.Serialization;

// NOTE: When adding new button types, you MUST add them to the end of the enum or else the rest will all shift
public enum ButtonType
{
  Submit,
  A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
  One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Zero,
  Space, Backspace,
}

public class TypeWriterPuzzle : Puzzle
{
  private TextMeshProUGUI puzzle_text;

  [Header("Puzzle")]
  [Tooltip("This defines the possible puzzle solutions.")]
  public static List<string> Solutions = new List<string>
  {
    "BLUE RED YELLOW",
  };

  private Dictionary<ButtonType, string> ButtonToString = new Dictionary<ButtonType, string>
    {
        { ButtonType.Submit, "" },
        { ButtonType.A, "A" },
        { ButtonType.B, "B" },
        { ButtonType.C, "C" },
        { ButtonType.D, "D" },
        { ButtonType.E, "E" },
        { ButtonType.F, "F" },
        { ButtonType.G, "G" },
        { ButtonType.H, "H" },
        { ButtonType.I, "I" },
        { ButtonType.J, "J" },
        { ButtonType.K, "K" },
        { ButtonType.L, "L" },
        { ButtonType.M, "M" },
        { ButtonType.N, "N" },
        { ButtonType.O, "O" },
        { ButtonType.P, "P" },
        { ButtonType.Q, "Q" },
        { ButtonType.R, "R" },
        { ButtonType.S, "S" },
        { ButtonType.T, "T" },
        { ButtonType.U, "U" },
        { ButtonType.V, "V" },
        { ButtonType.W, "W" },
        { ButtonType.X, "X" },
        { ButtonType.Y, "Y" },
        { ButtonType.Z, "Z" },
        { ButtonType.One, "1" },
        { ButtonType.Two, "2" },
        { ButtonType.Three, "3" },
        { ButtonType.Four, "4" },
        { ButtonType.Five, "5" },
        { ButtonType.Six, "6" },
        { ButtonType.Seven, "7" },
        { ButtonType.Eight, "8" },
        { ButtonType.Nine, "9" },
        { ButtonType.Zero, "0" },
        { ButtonType.Space, " " },
        { ButtonType.Backspace, ""},
    };

  [SerializeField] public string pressed = "";
  [SerializeField] private string _solution = "";
  [SerializeField] private string _answer = "";

  private List<SecretObjectiveID> broadcastedObjectives = new List<SecretObjectiveID>();
  private TypeWriter typeWriter;

  public override void Awake()
  {
    base.Awake();
    puzzleName = "TypeWriter";
    EventManager.AddListener<InteractEvent>(OnButtonPressed);
    typeWriter = GameObject.Find("Typewriter").GetComponent<TypeWriter>();
  }

  private void OnDestroy()
  {
    EventManager.RemoveListener<InteractEvent>(OnButtonPressed);
  }

  void Start()
  {
    puzzle_text = GameObject.Find("Puzzle_Text").GetComponentInChildren<TextMeshProUGUI>();

    // Demo puzzle ID = 0
    UpdateSolution(0);
  }

  void UpdateSolution(int puzzleId)
  {
    _solution = Solutions[puzzleId];
  }

  public void OnButtonPressed(InteractEvent evt)
  {
    Button button = evt.gameObject.GetComponent<Button>();
    if (button)
    {
      ButtonPressed(button);
    }
  }

  public void ButtonPressed(Button button)
  {
    pressed += (ButtonToString[button.buttonType]);
    typeWriter.playKeydownClip();
    if (_answer.Length == 0)
    {
      puzzle_text.text = "";
    }
    switch (button.buttonType)
    {
      case ButtonType.Backspace:
        if (_answer.Length > 0)
        {
          _answer = _answer.Substring(0, _answer.Length - 1);
          puzzle_text.text = puzzle_text.text.Substring(0, puzzle_text.text.Length - 1);
        }
        break;
      case ButtonType.Submit:
        if (CheckAnswer())
        {
          levelManager.audioController.playSuccess();
          Complete();
        }
        else
        {
          _answer = "";
          puzzle_text.text = "Mistake!";
          levelManager.audioController.playMistake();
        }
        break;
      default:
        _answer += ButtonToString[button.buttonType];
        puzzle_text.text += ButtonToString[button.buttonType];
        break;
    }
    if (pressed.Contains("FOOL") && !broadcastedObjectives.Contains(SecretObjectiveID.TypeFOOL))
    {
      SecretObjectiveEvent s = new SecretObjectiveEvent();
      s.id = SecretObjectiveID.TypeFOOL;
      s.status = true;
      EventManager.Broadcast(s);
      broadcastedObjectives.Add(SecretObjectiveID.TypeFOOL);
    }
  }

  private bool CheckAnswer()
  {
    return _answer == _solution;
  }
}
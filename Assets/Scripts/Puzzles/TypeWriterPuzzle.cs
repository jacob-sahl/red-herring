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
  Space, Backspace, Void, Query,
}

public class TypeWriterPuzzle : Puzzle
{
  private TextMeshProUGUI puzzle_text;
  private TextMeshProUGUI underscore_text;

  [Header("Puzzle")]
  [Tooltip("This defines the possible puzzle solutions.")]

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
      { ButtonType.Backspace, "" },
      { ButtonType.Void, "" },
      { ButtonType.Query, "" },
    };

  private Dictionary<ButtonType, string> ButtonToAnimTrigger = new Dictionary<ButtonType, string>
    {
      { ButtonType.Submit, "KeyPressSubmit" },
      { ButtonType.A, "KeyPressA" },
      { ButtonType.B, "KeyPressB" },
      { ButtonType.C, "KeyPressC" },
      { ButtonType.D, "KeyPressD" },
      { ButtonType.E, "KeyPressE" },
      { ButtonType.F, "KeyPressF" },
      { ButtonType.G, "KeyPressG" },
      { ButtonType.H, "KeyPressH" },
      { ButtonType.I, "KeyPressI" },
      { ButtonType.J, "KeyPressJ" },
      { ButtonType.K, "KeyPressK" },
      { ButtonType.L, "KeyPressL" },
      { ButtonType.M, "KeyPressM" },
      { ButtonType.N, "KeyPressN" },
      { ButtonType.O, "KeyPressO" },
      { ButtonType.P, "KeyPressP" },
      { ButtonType.Q, "KeyPressQ" },
      { ButtonType.R, "KeyPressR" },
      { ButtonType.S, "KeyPressS" },
      { ButtonType.T, "KeyPressT" },
      { ButtonType.U, "KeyPressU" },
      { ButtonType.V, "KeyPressV" },
      { ButtonType.W, "KeyPressW" },
      { ButtonType.X, "KeyPressX" },
      { ButtonType.Y, "KeyPressY" },
      { ButtonType.Z, "KeyPressZ" },
      { ButtonType.One, "KeyPress1" },
      { ButtonType.Two, "KeyPress2" },
      { ButtonType.Three, "KeyPress3" },
      { ButtonType.Four, "KeyPress4" },
      { ButtonType.Five, "KeyPress5" },
      { ButtonType.Six, "KeyPress6" },
      { ButtonType.Seven, "KeyPress7" },
      { ButtonType.Eight, "KeyPress8" },
      { ButtonType.Nine, "KeyPress9" },
      { ButtonType.Zero, "KeyPress0" },
      { ButtonType.Space, "KeyPressSpace" },
      { ButtonType.Backspace, "KeyPressBackspace" },
      { ButtonType.Void, "Void" },
      { ButtonType.Query, "KeyPressQuestion" },
    };

  [SerializeField] public string pressed = "";
  [SerializeField] private string _solution = "";
  [SerializeField] private string _answer = "";

  private List<SecretObjectiveID> broadcastedObjectives = new List<SecretObjectiveID>();
  private TypeWriter typeWriter;
  private Animator animator;

  public override void Awake()
  {
    base.Awake();
    puzzleName = "TypeWriter";
    EventManager.AddListener<InteractEvent>(OnButtonPressed);
    typeWriter = GetComponent<TypeWriter>();
  }

  private void OnDestroy()
  {
    EventManager.RemoveListener<InteractEvent>(OnButtonPressed);
  }

  void Start()
  {
    animator = GetComponentInChildren<Animator>();
    puzzle_text = GameObject.Find("PlayerText").GetComponent<TextMeshProUGUI>();
    underscore_text = GameObject.Find("TextUnderscores").GetComponent<TextMeshProUGUI>();
    string solution = GameController.Instance.puzzles[GameController.Instance.currentRound].solution;
    UpdateSolution(solution);
    setUnderscores(solution);
  }

  void UpdateSolution(string solution)
  {
    _solution = solution;
  }

  void setUnderscores(string solution)
  {
    string temp = "<mspace=12>";
    foreach (char c in solution)
    {
      if (c == ' ')
      {
        temp += " ";
      }
      else
      {
        temp += "_";
      }
    }
    temp += "</mspace>";
    underscore_text.text = temp;
  }

  public void OnButtonPressed(InteractEvent evt)
  {
    TypewriterButton button = evt.gameObject.GetComponent<TypewriterButton>();
    if (button)
    {
      ButtonPressed(button);
    }
  }

  public void ButtonPressed(TypewriterButton button)
  {
    pressed += (ButtonToString[button.buttonType]);
    typeWriter.playKeydownClip();
    animator.SetTrigger(ButtonToAnimTrigger[button.buttonType]);

    if (_answer.Length == 0)
    {
      setPuzzleText("");
    }
    switch (button.buttonType)
    {
      case ButtonType.Backspace:
        if (_answer.Length > 0)
        {
          _answer = _answer.Substring(0, _answer.Length - 1);
          setPuzzleText(_answer);
        }
        break;
      case ButtonType.Submit:
        if (CheckAnswer())
        {
          levelManager.audioController.playSuccess();
          animator.SetTrigger("CorrectInput");
          Complete();
        }
        else
        {
          _answer = "";
          setPuzzleText("Incorrect.");
          levelManager.audioController.playMistake();
          animator.SetTrigger("IncorrectInput");
        }
        break;
      case ButtonType.Query:
        queryString(_answer);
        break;
      default:
        _answer += ButtonToString[button.buttonType];
        setPuzzleText(_answer);
        break;
    }
  }

  private void setPuzzleText(string str)
  {
    puzzle_text.text = "<mspace=12>" + str + "</mspace>";
  }

  private void queryString(string str)
  {
    TypeWriterPuzzleInstance puzzle = GameController.Instance.getCurrentPuzzle();
    string result = "";
    if (puzzle.query.ContainsKey(str))
    {
      result = puzzle.query[str];
    }
    else
    {
      result = "???";
    }
    _answer = "";
    setPuzzleText(result);
  }

  public bool CheckAnswer()
  {
    return _answer == _solution;
  }
}
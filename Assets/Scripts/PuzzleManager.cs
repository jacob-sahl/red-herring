using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum ButtonType
{
  Submit,
  A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
  One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Zero,
  Space,
}

public class PuzzleManager : MonoBehaviour
{
  [Header("Parameters")]
  [Tooltip("Duration of the fade-to-black at the end of the game")]
  public float endSceneLoadDelay = 3f;

  [Tooltip("The canvas group of the fade-to-black screen")]
  public CanvasGroup endGameFadeCanvasGroup;

  [Header("Ending")]
  [Tooltip("This string has to be the name of the scene you want to load when game ends")]
  public string endSceneName = "EndScene";

  public float puzzleTime = 60f * 3f;
  [SerializeField] private float _timeLeft = 0f;
  [SerializeField] private bool puzzleStarted;
  private UIController uiController;
  private AudioController audioController;
  private TextMeshProUGUI puzzle_text;

  [Header("Puzzle")]
  [Tooltip("This defines the possible puzzle solutions.")]
  public static List<List<ButtonType>> Solutions = new List<List<ButtonType>> {
    new List<ButtonType> {
      ButtonType.B, ButtonType.L, ButtonType.U, ButtonType.E, ButtonType.Space,
      ButtonType.R, ButtonType.E, ButtonType.D, ButtonType.Space,
      ButtonType.Y, ButtonType.E, ButtonType.L, ButtonType.L, ButtonType.O, ButtonType.W
      },
  };

  private Dictionary<ButtonType, string> ButtonToString = new Dictionary<ButtonType, string> {
    {ButtonType.A, "A"},
    {ButtonType.B, "B"},
    {ButtonType.C, "C"},
    {ButtonType.D, "D"},
    {ButtonType.E, "E"},
    {ButtonType.F, "F"},
    {ButtonType.G, "G"},
    {ButtonType.H, "H"},
    {ButtonType.I, "I"},
    {ButtonType.J, "J"},
    {ButtonType.K, "K"},
    {ButtonType.L, "L"},
    {ButtonType.M, "M"},
    {ButtonType.N, "N"},
    {ButtonType.O, "O"},
    {ButtonType.P, "P"},
    {ButtonType.Q, "Q"},
    {ButtonType.R, "R"},
    {ButtonType.S, "S"},
    {ButtonType.T, "T"},
    {ButtonType.U, "U"},
    {ButtonType.V, "V"},
    {ButtonType.W, "W"},
    {ButtonType.X, "X"},
    {ButtonType.Y, "Y"},
    {ButtonType.Z, "Z"},
    {ButtonType.One, "1"},
    {ButtonType.Two, "2"},
    {ButtonType.Three, "3"},
    {ButtonType.Four, "4"},
    {ButtonType.Five, "5"},
    {ButtonType.Six, "6"},
    {ButtonType.Seven, "7"},
    {ButtonType.Eight, "8"},
    {ButtonType.Nine, "9"},
    {ButtonType.Zero, "0"},
    {ButtonType.Space, " "},
  };

  [SerializeField] private List<ButtonType> _pressed = new List<ButtonType> { };

  [SerializeField] private List<ButtonType> _solution;
  [SerializeField] private List<ButtonType> _answer = new List<ButtonType> { };

  public List<Instructor> instructors = new List<Instructor> { };
  public bool gameIsEnding;
  private float _timeLoadEndGameScene;

  void Awake()
  {
    EventManager.AddListener<InteractEvent>(OnButtonPressed);
  }

  void Start()
  {
    _timeLeft = puzzleTime;

    puzzleStarted = true;
    audioController = GameObject.Find("AudioManager").GetComponent<AudioController>();
    uiController = GameObject.Find("Hud").GetComponent<UIController>();
    puzzle_text = GameObject.Find("Puzzle_Text").GetComponentInChildren<TextMeshProUGUI>();

    // Demo puzzle ID = 0
    UpdateSolution(0);
    GameStartEvent gameStartEvent = new GameStartEvent();
    EventManager.Broadcast(gameStartEvent);
    Debug.Log("GameStartEvent broadcasted");
  }

  // Update solution to solution_num
  void UpdateSolution(int puzzleId)
  {
    _solution = Solutions[puzzleId];
  }

  // Update is called once per frame
  void Update()
  {
    if (puzzleStarted)
    {
      _timeLeft -= Time.deltaTime;
      uiController.displayTime(_timeLeft);

      if (_timeLeft <= 0)
      {
        Time.timeScale = 0;
        // ends the game
        EndPuzzle();
      }

      _timeLeft = puzzleTime;
      puzzleStarted = true;
      audioController = GameObject.Find("AudioManager").GetComponent<AudioController>();
      uiController = GameObject.Find("Hud").GetComponent<UIController>();
    }

    if (gameIsEnding)
    {
      float timeRatio = 1 - (_timeLoadEndGameScene - Time.time) / endSceneLoadDelay;
      endGameFadeCanvasGroup.alpha = timeRatio;

      if (Time.time >= _timeLoadEndGameScene)
      {
        GameController.Instance.LoadEndScene();
        gameIsEnding = false;
      }
    }

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
    _pressed.Add(button.buttonType);
    if (_answer.Count == 0)
    {
      puzzle_text.text = "";
    }
    switch (button.buttonType)
    {
      case ButtonType.Submit:
        if (CheckAnswer())
        {
          audioController.playSuccess();
          EndPuzzle();
        }
        else
        {
          _answer.Clear();
          puzzle_text.text = "Mistake!";
          audioController.playMistake();
        }
        break;
      default:
        _answer.Add(button.buttonType);
        puzzle_text.text += ButtonToString[button.buttonType];
        break;
    }
  }

  private bool CheckAnswer()
  {
    if (_answer.Count == _solution.Count)
    {
      for (int i = 0; i < _answer.Count; i++)
      {
        if (_answer[i] != _solution[i])
        {
          return false;
        }
      }

      return true;
    }
    else
    {
      return false;
    }
  }

  private void SetupInstructors()
  {
    instructors[0].name = "Instructor 0";
    instructors[1].name = "Instructor 1";
    instructors[2].name = "Instructor 2";
    this.instructors[0].SetupSecretGoal(pressed =>
    {
      return false;
    });
    this.instructors[1].SetupSecretGoal(pressed => pressed.Contains(ButtonType.G));
    this.instructors[2].SetupSecretGoal(pressed => !pressed.Contains(ButtonType.G));
  }

  private void EndPuzzle()
  {
    puzzleStarted = false;

    Debug.Log($"Box Opened: {CheckAnswer()}");
    List<string> winners = new List<string>();
    if (CheckAnswer())
    {
      winners.Add("Operator");
    }
    foreach (var instructor in instructors)
    {
      // Debug.Log($"{instructor.name}'s secret goal: {instructor.CheckSecretGoal(_pressed)}");
      // if (instructor.CheckSecretGoal(_pressed))
      // {
      //   winners.Add($"{instructor.name}");
      // }
    }
    string gameOverText = "Game Over. Winners:";
    foreach (var winner in winners)
    {
      gameOverText += $" {winner}";
    }

    GameOverEvent gameOverEvent = new GameOverEvent();
    gameOverEvent.PuzzleSolved = CheckAnswer();
    gameOverEvent.EndGameMessage = gameOverText;
    EventManager.Broadcast(gameOverEvent);
    uiController.displayText(gameOverText);
    FadeOut();
  }

  private void FadeOut()
  {
    // unlocks the cursor before leaving the scene, to be able to click buttons
    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;

    // Remember that we need to load the appropriate end scene after a delay
    gameIsEnding = true;
    endGameFadeCanvasGroup.gameObject.SetActive(true);

    _timeLoadEndGameScene = Time.time + endSceneLoadDelay;
  }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonType
{
  Key1,
  Key2,
  Key3,
  Key4,
  Reset,
  Green,
  Invalid,
}

public enum Solutions
{
  puzzle0,
  puzzle1
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
  private GameObject puzzle_text;

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
    // SetupInstructors();

    _timeLeft = puzzleTime;
    puzzleStarted = true;
    audioController = GameObject.Find("AudioManager").GetComponent<AudioController>();
    uiController = GameObject.Find("Hud").GetComponent<UIController>();
    puzzle_text = GameObject.Find("Puzzle_Text");
    Debug.Log(puzzle_text);
    puzzle_text.SetActive(false);

    // calls SetupInstructors in UpdateSolution as well
    UpdateSolution(_solution, Solutions.puzzle0);

  }

  // Update solution to solution_num
  void UpdateSolution(List<ButtonType> _solution, Solutions puzzle)
  {
    switch (puzzle)
    {
      case Solutions.puzzle0:
        _solution = new List<ButtonType> { ButtonType.Key2, ButtonType.Key3, ButtonType.Key4, ButtonType.Key1, ButtonType.Key4, ButtonType.Key2, ButtonType.Key2 };
        break;
      case Solutions.puzzle1:
        break;
    }
    Debug.Log(_solution);
    SetupInstructors(puzzle);
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

  ButtonType GetButtonTypeFromTag(string buttonTag)
  {
    switch (buttonTag)
    {
      case "key1":
        return ButtonType.Key1;
      case "key2":
        return ButtonType.Key2;
      case "key3":
        return ButtonType.Key3;
      case "key4":
        return ButtonType.Key4;
      case "reset":
        return ButtonType.Reset;
      case "green":
        return ButtonType.Green;
      default:
        return ButtonType.Invalid;
    }
  }

  public void OnButtonPressed(InteractEvent evt)
  {
    ButtonPressed(evt.ObjectTag);
  }

  public void ButtonPressed(string buttonTag)
  {
    ButtonType buttonType = GetButtonTypeFromTag(buttonTag);
    _pressed.Add(buttonType);
    switch (buttonType)
    {
      case ButtonType.Key1:
      case ButtonType.Key2:
      case ButtonType.Key3:
      case ButtonType.Key4:
        _answer.Add(buttonType);
        if (_answer.Count > _solution.Count || _answer[_answer.Count - 1] != _solution[_answer.Count - 1])
        {
          audioController.playMistake();
        }

        break;
      case ButtonType.Reset:
        //_answer.Clear();
        if (CheckAnswer())
        {
          audioController.playSuccess();
          EndPuzzle();
        }
        else
        {
          _answer.Clear();
        }
        break;
      case ButtonType.Green:
        break;
    }

    // make mistake text turn on or off
    Debug.Log(_answer.Count);
    if (_answer.Count == 0)
    {
      //break;
      // make puzzle text visible
      puzzle_text.SetActive(true);
      Debug.Log("puzzle text active");
    }
    else
    {
      // break;
      // make puzzle text invisible
      puzzle_text.SetActive(false);
      Debug.Log("puzzle text inactive");
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

  private void SetupInstructors(Solutions puzzle)
  {
    this.instructors[0].name = "Instructor 0";
    this.instructors[1].name = "Instructor 1";
    this.instructors[2].name = "Instructor 2";
    switch (puzzle)
    {
      case Solutions.puzzle0:
        this.instructors[0].SetupSecretGoal(pressed =>
        {
          if (pressed.Count > 4)
          {
            return pressed[2] == ButtonType.Reset && pressed[3] == ButtonType.Reset;
          }
          else
          {
            return false;
          }
        });

        this.instructors[1].SetupSecretGoal(pressed => pressed.Contains(ButtonType.Green));
        this.instructors[2].SetupSecretGoal(pressed => !pressed.Contains(ButtonType.Green));
        break;
      case Solutions.puzzle1:
        break;
    }
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
      Debug.Log($"{instructor.name}'s secret goal: {instructor.CheckSecretGoal(_pressed)}");
      if (instructor.CheckSecretGoal(_pressed))
      {
        winners.Add($"{instructor.name}");
      }
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
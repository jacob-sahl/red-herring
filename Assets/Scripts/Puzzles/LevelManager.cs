using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;


public class LevelManager : MonoBehaviour
{
  [Header("Parameters")]
  [Tooltip("Duration of the fade-to-black at the end of the game")]
  public float endSceneLoadDelay = 3f;

  [Tooltip("The canvas group of the fade-to-black screen")]
  public CanvasGroup endGameFadeCanvasGroup;

  [Header("Ending")]
  [Tooltip("This string has to be the name of the scene you want to load when game ends")]
  public string endSceneName = "RoundEnd";

  private float puzzleTime;
  private float _completionTime;
  [SerializeField] private float _timeLeft;
  [SerializeField] private bool puzzleStarted;
  private UIController uiController;
  public AudioController audioController;
  private PlayerManager playerManager;
  private GameController gameController;
  public bool gameIsEnding;
  private float _timeLoadEndGameScene;
  public List<Puzzle> puzzles = new List<Puzzle> { };

  void Start()
  {
    gameController = GameController.Instance;
    audioController = GameObject.Find("AudioManager").GetComponent<AudioController>();
    uiController = GameObject.Find("Hud").GetComponent<UIController>();
    playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    puzzleTime = gameController.minutesPerRound * 60f;
    _timeLeft = puzzleTime;
    puzzleStarted = true;
    LevelStartEvent levelStartEvent = new LevelStartEvent();
    EventManager.Broadcast(levelStartEvent);
    Debug.Log("GameStartEvent broadcasted");
  }

  // Update is called once per frame
  void Update()
  {
    if (puzzleStarted)
    {
      _timeLeft -= Time.deltaTime;

      if (_timeLeft > 0)
      {
        uiController.displayTime(_timeLeft);
      }
      else
      {
        // ends the game
        EndLevel();
      }
    }

    if (gameIsEnding)
    {
      float timeRatio = 1 - (_timeLoadEndGameScene - Time.time) / endSceneLoadDelay;
      endGameFadeCanvasGroup.alpha = timeRatio;

      if (Time.time >= _timeLoadEndGameScene)
      {
        if (GameController.Instance.currentRound == 3)
        {
          GameController.Instance.LoadGameEndScene();
        }
        else
        {
          GameController.Instance.LoadEndScene();
        }
        gameIsEnding = false;
      }
    }
  }

  public void addPuzzle(Puzzle puzzle)
  {
    puzzles.Add(puzzle);
    puzzle.SetCompleteCallback(() => onPuzzleComplete(puzzle));
  }

  private void onPuzzleComplete(Puzzle puzzle)
  {
    Debug.Log("Puzzle complete");
    if (canEndLevel())
    {
      EndLevel();
    }
  }

  private bool canEndLevel()
  {
    foreach (Puzzle puzzle in puzzles)
    {
      if (!puzzle.isComplete)
      {
        return false;
      }
    }

    return true;
  }

  private void checkCompletionTime()
  {
    _completionTime = _timeLeft;
    // Checking digits of completion time
    int minutes = Mathf.FloorToInt(_completionTime / 60);
    int seconds = Mathf.FloorToInt(_completionTime % 60);
    // Debug.Log("Completion Time, Minutes: " + minutes + " Seconds: " + seconds);
    if (minutes.ToString().Contains("3") || seconds.ToString().Contains("3"))
    {
      SecretObjectiveEvent e = new SecretObjectiveEvent();
      e.id = SecretObjectiveID.SolveWithThreeOnTimer;
      e.status = true;
      EventManager.Broadcast(e);
    }
    if (minutes >= 3)
    {
      SecretObjectiveEvent e = new SecretObjectiveEvent();
      e.id = SecretObjectiveID.SolveQuickly;
      e.status = true;
      EventManager.Broadcast(e);
    }
  }

  private void checkObjectMovement()
  {
    Debug.Log("Checking object movement");
    GameObject gramophone = GameObject.Find("Gramophone").gameObject;
    if (!gramophone.GetComponent<HasMoved>().hasMoved())
    {
      Debug.Log("Gramophone has not moved");
      SecretObjectiveEvent e = new SecretObjectiveEvent();
      e.id = SecretObjectiveID.StationaryGramophone;
      e.status = true;
      EventManager.Broadcast(e);
    }
  }

  private void EndLevel()
  {
    puzzleStarted = false;
    checkCompletionTime();
    checkObjectMovement();
    List<int> pointsToAdd = new List<int> { 0, 0, 0, 0 };
    string roundEndText = "In this round, ";
    foreach (var puzzle in puzzles)
    {
      if (puzzle.isComplete)
      {
        roundEndText += $"the puzzle was completed. \n";
        // Calculate points each player earned this round
        for (int i = 0; i < pointsToAdd.Count; i++)
        {
          pointsToAdd[i] += 4;
        }
      }
      else
      {
        roundEndText += $"the puzzle was not completed. \n";
      }
      foreach (SecretObjective secret in gameController.currentSecretObjectives)
      {
        roundEndText += $"\n<b>Player {secret.player.playerId + 1}'s Secrect Objective:</b> {secret.description}\n";
        if (secret.completed)
        {
          pointsToAdd[secret.player.playerId] += 4;
          // TEMPORARY:
          roundEndText += $"<b>Status:</b> Complete!\n";
          // TEMP ^
          for (int i = 0; i < pointsToAdd.Count; i++)
          {
            pointsToAdd[i] -= 1;
          }
        }
        else
        {
          roundEndText += "<b>Status:</b> Incomplete!\n";
        }
      }
      if (puzzle.isComplete)
      {
        // Add points to each player's total
        for (int i = 0; i < pointsToAdd.Count; i++)
        {
          playerManager.players[i].points += pointsToAdd[i];
        }
      }
    }
    Debug.Log(roundEndText);

    LevelEndEvent levelEndEvent = new LevelEndEvent();
    levelEndEvent.endMessage = roundEndText;
    EventManager.Broadcast(levelEndEvent);

    if (GameController.Instance.currentRound == 3)
    {
      EndGame();
    }

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

  private void EndGame()
  {
    string gameEndText = "In this game,\n";
    for (int i = 0; i < playerManager.players.Count; i++)
    {
      gameEndText += $"Player {i + 1} earned {playerManager.players[i].points} points\n";
    }
    GameEndEvent gameEndEvent = new GameEndEvent();
    gameEndEvent.endMessage = gameEndText;
    EventManager.Broadcast(gameEndEvent);
  }
}
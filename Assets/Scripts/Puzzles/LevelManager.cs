using System.Collections.Generic;
using UnityEngine;

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

  [SerializeField] private float _timeLeft;
  [SerializeField] private bool puzzleStarted;
  public AudioController audioController;
  public bool gameIsEnding;
  public List<Puzzle> puzzles = new();
  private float _completionTime;
  private float _timeLoadEndGameScene;
  private GameController gameController;
  private PlayerManager playerManager;

  private float puzzleTime;
  private UIController uiController;

  private void Start()
  {
    gameController = GameController.Instance;
    audioController = GameObject.Find("AudioManager").GetComponent<AudioController>();
    uiController = GameObject.Find("Hud").GetComponent<UIController>();
    playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    puzzleTime = gameController.minutesPerRound * 60f;
    _timeLeft = puzzleTime;
    puzzleStarted = true;
    var levelStartEvent = new LevelStartEvent();
    EventManager.Broadcast(levelStartEvent);
    Debug.Log("GameStartEvent broadcasted");
  }

  // Update is called once per frame
  private void Update()
  {
    if (puzzleStarted)
    {
      _timeLeft -= Time.deltaTime;

      if (_timeLeft > 0)
        uiController.displayTime(_timeLeft);
      else
        // ends the game
        EndLevel();
    }

    if (gameIsEnding)
    {
      var timeRatio = 1 - (_timeLoadEndGameScene - Time.time) / endSceneLoadDelay;
      endGameFadeCanvasGroup.alpha = timeRatio;

      if (Time.time >= _timeLoadEndGameScene)
      {
        if (GameController.Instance.currentRound == 3)
          GameController.Instance.LoadGameEndScene();
        else
          GameController.Instance.LoadEndScene();
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
    if (canEndLevel()) EndLevel();
  }

  private bool canEndLevel()
  {
    foreach (var puzzle in puzzles)
      if (!puzzle.isComplete)
        return false;

    return true;
  }

  private void checkCompletionTime()
  {
    _completionTime = _timeLeft;
    // Checking digits of completion time
    var minutes = Mathf.FloorToInt(_completionTime / 60);
    var seconds = Mathf.FloorToInt(_completionTime % 60);
    // Debug.Log("Completion Time, Minutes: " + minutes + " Seconds: " + seconds);
    if (minutes.ToString().Contains("3") || seconds.ToString().Contains("3"))
    {
      var e = new SecretObjectiveEvent();
      e.id = SecretObjectiveID.SolveWithThreeOnTimer;
      e.status = true;
      EventManager.Broadcast(e);
    }

    if (minutes >= 3)
    {
      var e = new SecretObjectiveEvent();
      e.id = SecretObjectiveID.SolveQuickly;
      e.status = true;
      EventManager.Broadcast(e);
    }
  }

  private void checkObjectMovement()
  {
    Debug.Log("Checking object movement");
    var gramophone = GameObject.Find("Gramophone").gameObject;
    if (!gramophone.GetComponent<HasMoved>().hasMoved())
    {
      Debug.Log("Gramophone has not moved");
      var e = new SecretObjectiveEvent();
      e.id = SecretObjectiveID.StationaryGramophone;
      e.status = true;
      EventManager.Broadcast(e);
    }
  }

  public void EndLevel()
  {
    puzzleStarted = false;
    checkCompletionTime();
    checkObjectMovement();
    var pointStages = new List<List<int>> { };
    LevelEndEvent levelEndEvent = new LevelEndEvent();
    levelEndEvent.messages = new List<string>();

    foreach (var puzzle in puzzles)
    {
      if (puzzle.isComplete)
      {
        levelEndEvent.puzzleCompleted = true;
        pointStages.Add(new List<int> { 4, 4, 4, 4 });
      }
      else
      {
        levelEndEvent.puzzleCompleted = false;
      }

      foreach (var secret in gameController.currentSecretObjectives)
      {
        if (secret.completed)
        {
          var points = new List<int> { 0, 0, 0, 0 };
          points[secret.player.playerId] += 4;
          for (var i = 0; i < points.Count; i++) points[i] -= 1;
          pointStages.Add(points);
          levelEndEvent.messages.Add("Player " + (secret.player.playerId + 1) + " completed their secret objective: " + secret.description);
        }
        else
        {
          pointStages.Add(new List<int> { 0, 0, 0, 0 });
          levelEndEvent.messages.Add("Player " + (secret.player.playerId + 1) + " did not complete their secret objective: " + secret.description);
        }
      }

      if (puzzle.isComplete)
        // Reset points this round
        for (var i = 0; i < 4; i++)
        {
          playerManager.players[i].pointsThisRound = 0;
        }

      // Add points to each player's total
      foreach (var points in pointStages)
      {
        for (var i = 0; i < points.Count; i++)
        {
          playerManager.players[i].points += points[i];
          playerManager.players[i].pointsThisRound += points[i];
        }
      }
    }
    levelEndEvent.pointStages = pointStages;

    EventManager.Broadcast(levelEndEvent);

    if (GameController.Instance.currentRound == 3) EndGame();

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
    var gameEndText = "In this game,\n";
    for (var i = 0; i < playerManager.players.Count; i++)
      gameEndText += $"Player {i + 1} earned {playerManager.players[i].points} points\n";
    var gameEndEvent = new GameEndEvent();
    gameEndEvent.endMessage = gameEndText;
    EventManager.Broadcast(gameEndEvent);
  }
}
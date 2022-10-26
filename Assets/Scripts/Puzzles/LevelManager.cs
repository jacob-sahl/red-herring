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
  public string endSceneName = "End";

  private float puzzleTime;
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
    puzzleTime = GameController.Instance.minutesPerRound * 60f;
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
      uiController.displayTime(_timeLeft);

      if (_timeLeft <= 0)
      {
        // ends the game
        EndLevel();
      }

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

  public void addPuzzle(Puzzle puzzle)
  {
    puzzles.Add(puzzle);
    puzzle.SetCompleteCallback(() => onPuzzleComplete(puzzle));
  }

/*    public void Pause(bool pause)
    {
        GameObject PauseText = GameObject.Find("PauseText");
        Debug.Log("checking for pause");
        if (pause && Time.timeScale == 0.0f)
        {
            Time.timeScale = 1f;
            PauseText.SetActive(false);
        }
        if (pause && Time.timeScale == 1.0f)
        {
            Time.timeScale = 0f;
            PauseText.SetActive(true);
        }
    }*/

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

  private void EndLevel()
  {
    puzzleStarted = false;
    List<int> pointsToAdd = new List<int> { 0, 0, 0, 0 };
    string roundEndText = "In this round, ";
    foreach (var puzzle in puzzles)
    {
      if (puzzle.isComplete)
      {
        roundEndText += $"Puzzle {puzzle.name} is complete. \n";
        // Calculate points each player earned this round
        for (int i = 0; i < pointsToAdd.Count; i++)
        {
          pointsToAdd[i] += 4;
        }
        foreach (SecretObjective secret in gameController.currentSecretObjectives)
        {
          if (secret.completed)
          {
            pointsToAdd[secret.player.playerId] += 4;
            // TEMPORARY:
            roundEndText += $"Player {secret.player.playerId + 1} completed their secret objective, they win!\n";
            // TEMP ^
            for (int i = 0; i < pointsToAdd.Count; i++)
            {
              pointsToAdd[i] -= 1;
            }
          }
        }
        // Add points to each player's total
        for (int i = 0; i < pointsToAdd.Count; i++)
        {
          playerManager.players[i].points += pointsToAdd[i];
        }
      }
      else
      {
        roundEndText += $"Puzzle {puzzle.name} is not complete. \n";
      }
    }
    Debug.Log(roundEndText);

    LevelEndEvent levelEndEvent = new LevelEndEvent();
    levelEndEvent.endMessage = roundEndText;
    EventManager.Broadcast(levelEndEvent);

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
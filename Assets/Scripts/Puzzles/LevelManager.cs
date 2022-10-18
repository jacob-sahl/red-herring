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

  public float puzzleTime = 60f * 3f;
  [SerializeField] private float _timeLeft = 0f;
  [SerializeField] private bool puzzleStarted;
  private UIController uiController;
  public AudioController audioController;
  private PlayerManager playerManager;
  private GameController gameController;
  public List<SecretObjective> secretObjectives;
  public bool gameIsEnding;
  private float _timeLoadEndGameScene;
  public List<Puzzle> puzzles = new List<Puzzle> { };

  void Start()
  {

    gameController = GameObject.Find("GameController").GetComponent<GameController>();
    audioController = GameObject.Find("AudioManager").GetComponent<AudioController>();
    uiController = GameObject.Find("Hud").GetComponent<UIController>();
    playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    assignSecretObjectives();

    _timeLeft = puzzleTime;
    puzzleStarted = true;
    LevelStartEvent levelStartEvent = new LevelStartEvent();
    EventManager.Broadcast(levelStartEvent);
    Debug.Log("GameStartEvent broadcasted");
  }
  void assignSecretObjectives()
  {
    secretObjectives = new List<SecretObjective>();
    // Randomize
    List<int> informants = new List<int> { 0, 1, 2, 3 };
    // remove the current detective
    informants.Remove(gameController.detectiveOrder[gameController.currentRound - 1]);
    List<int> order = new List<int>();
    for (int i = 0; i < 3; i++)
    {
      int index = Mathf.FloorToInt(Random.Range(0, informants.Count));
      order.Add(informants[index]);
      informants.Remove(informants[index]);
    }
    // Hardcoded for now. LATER: secret objectives depend on the puzzle instance
    secretObjectives.Add(new SecretObjective(
        playerManager.players[order[0]],
        "Get the detective to look out of the window for three consecutive seconds.",
        SecretObjectiveID.LookThroughWindow
    ));
    secretObjectives.Add(new SecretObjective(
        playerManager.players[order[1]],
        "Get the detective to turn the typewriter upside-down.",
        SecretObjectiveID.InvertTypewriter
    ));
    secretObjectives.Add(new SecretObjective(
        playerManager.players[order[2]],
        "Get the detective to type 'FOOL' into the typewriter.",
        SecretObjectiveID.TypeFOOL
    ));
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
        foreach (SecretObjective secret in secretObjectives)
        {
          if (secret.completed)
          {
            pointsToAdd[secret.player.playerId] += 4;
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
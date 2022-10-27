using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameController : MonoBehaviour
{
  public static GameController Instance { get; private set; }
  public PlayerManager PlayerManager;
  public LevelManager levelManager;
  [SerializeField] public bool forceStart = false;
  public string _roundEndText;

  [Header("Main Scene")]
  [Tooltip("This string has to be the name of the scene you want to load when starting a round")]
  public string mainSceneName;
  public List<int> detectiveOrder;
  // NOTE: currentRound is 1-indexed (starts at 1 on round 1, NOT 0)
  public int currentRound;
  public int minutesPerRound = 3;
  public List<TypeWriterPuzzleInstance> puzzles = new List<TypeWriterPuzzleInstance> {
    new TypeWriterPuzzleInstance(
      "BLUE RED YELLOW",
      new List<(SecretObjectiveID, string)> {
        (SecretObjectiveID.LookThroughWindow, "Get the detective to look out of the window for three consecutive seconds."),
        (SecretObjectiveID.InvertTypewriter, "Get the detective to turn the typewriter upside-down."),
        (SecretObjectiveID.TypeFOOL, "Get the detective to type 'FOOL' into the typewriter."),
      },
      new List<string> {
        "The solution is in alphabetical order.",
        "The solution is very colourful.",
        "The solution is not secondary.",
      }
    ),
    new TypeWriterPuzzleInstance(
      "ONE 2 THREE",
      new List<(SecretObjectiveID, string)> {
        (SecretObjectiveID.LookThroughWindow, "Get the detective to look out of the window for three consecutive seconds."),
        (SecretObjectiveID.DropCorrect, "Get the detective to drop the typewriter while the correct solution is written out (you must get them to do this before they hit 'submit')."),
        (SecretObjectiveID.TypeFIVE, "Get the detective to type 'FIVE' into the typewriter."),
      },
      new List<string> {
        "The answer involves counting.",
        "The second word of the answer is a number.",
        "... Four!",
      }
    ),
  };
  public List<SecretObjective> currentSecretObjectives;
  public List<string> currentClues;
  void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(this);
    }
    else
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }

    // Randomized detective order
    // List<int> players = new List<int> { 0, 1, 2, 3 };
    // detectiveOrder = new List<int>();
    // for (int i = 0; i < players.Count; i++)
    // {
    //   int index = Mathf.FloorToInt(Random.Range(0, players.Count));
    //   detectiveOrder.Add(players[index]);
    //   players.Remove(players[index]);
    // }

    // Deterministic detective order
    detectiveOrder = new List<int> { 0, 1, 2, 3 };
    currentRound = -1;
    EventManager.AddListener<LevelStartEvent>(onGameStart);
    EventManager.AddListener<LevelEndEvent>(onLevelEnd);
  }

  // Start is called before the first frame update
  void Start()
  {
    PlayerManager = PlayerManager.Instance;
  }

  private void OnDestroy()
  {
    EventManager.RemoveListener<LevelStartEvent>(onGameStart);
    EventManager.RemoveListener<LevelEndEvent>(onLevelEnd);
  }

  public int getCurrentDetective()
  {
    return detectiveOrder[currentRound];
  }

  private List<int> getRandomSOAssignment()
  {
    // Randomize
    List<int> informants = new List<int> { 0, 1, 2, 3 };
    // remove the current detective
    informants.Remove(detectiveOrder[currentRound]);
    List<int> assignment = new List<int>();
    for (int i = 0; i < 3; i++)
    {
      int index = Mathf.FloorToInt(Random.Range(0, informants.Count));
      assignment.Add(informants[index]);
      informants.Remove(informants[index]);
    }
    return assignment;
  }

  void assignSecretObjectives()
  {
    // Destroy previous secret objectives
    {
      while (currentSecretObjectives != null && currentSecretObjectives.Count > 0)
      {
        SecretObjective so = currentSecretObjectives[currentSecretObjectives.Count - 1];
        so.Deconstruct();
        currentSecretObjectives.Remove(so);
      }
    }

    TypeWriterPuzzleInstance puzzle = puzzles[currentRound];
    currentSecretObjectives = new List<SecretObjective>();
    currentClues = new List<string>();
    List<int> order = getRandomSOAssignment();

    for (int i = 0; i < 3; i++)
    {
      SecretObjective so = new SecretObjective(
        PlayerManager.getPlayerByID(order[i]),
        puzzle.secrets[i].Item2,
        puzzle.clues[i],
        puzzle.secrets[i].Item1
    );
      // Are these necessary?
      currentSecretObjectives.Add(so);
      currentClues.Add(puzzle.clues[i]);
    }
  }

  public SecretObjective getPlayersSecretObjective(int playerId)
  {
    foreach (SecretObjective secret in currentSecretObjectives)
    {
      if (secret.player.playerId == playerId)
      {
        return secret;
      }
    }
    return null;
  }

  public void updateMinutesPerRound(string value)
  {
    minutesPerRound = int.Parse(value);
  }

  public void LoadScene(string name)
  {
    SceneManager.LoadScene(name);
  }

  public void LoadMenuScene()
  {
    LoadScene("Menu");
  }

  public void LoadEndScene()
  {
    LoadScene("RoundEnd");
  }

  private bool checkCanStartGame()
  {
    return forceStart || PlayerManager.players.Count == 4;
  }
  public void LoadLevel()
  {
    if (checkCanStartGame())
    {
      LoadScene(mainSceneName);
    }
    else
    {
      Debug.Log("Not enough players");
    }
  }

  public bool IsGameEnding()
  {
    if (levelManager == null)
    {
      return false;
    }
    else
    {
      return levelManager.gameIsEnding;
    }
  }

  private void onGameStart(LevelStartEvent e)
  {
    Debug.Log("Game Start received by GameController");
    levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
  }

  public void SetupLevel()
  {
    // FOR DEV PURPOSES: (REMOVE ON BUILD)
    PlayerManager.fillPlayers();
    // ^
    currentRound++; // Must be done FIRST
    assignSecretObjectives();
    LevelSetupCompleteEvent e = new LevelSetupCompleteEvent();
    EventManager.Broadcast(e);
  }

  void onLevelEnd(LevelEndEvent e)
  {
    _roundEndText = e.endMessage;
  }
}
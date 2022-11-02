using System.Collections;
using System.Collections.Generic;
using Game;
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
  public string _gameEndText;

  [Header("Main Scene")]
  [Tooltip("This string has to be the name of the scene you want to load when starting a round")]
  public string mainSceneName;
  public List<int> detectiveOrder;
  // NOTE: currentRound is 1-indexed (starts at 1 on round 1, NOT 0)
  public int currentRound;
  public int minutesPerRound = 5;
  public float mouseSensitivity = 1f;
  public float objectRotationSpeed = 0.3f;
  public List<TypeWriterPuzzleInstance> puzzles = new List<TypeWriterPuzzleInstance> {
    new TypeWriterPuzzleInstance(
      TypeWriterPuzzleID.BlueRedYellow,
      "BLUE RED YELLOW",
      new List<(SecretObjectiveID, string)> {
        (SecretObjectiveID.LookThroughWindow, "Get the detective to look out of the window for three consecutive seconds."),
        (SecretObjectiveID.InvertTypewriter, "Get the detective to turn the typewriter upside-down."),
        (SecretObjectiveID.SpinGlobeThrice, "Get the detective to spin the globe around three times."),
      },
      new List<string> {
        "The solution is very colourful.",
        "There is a clue underneath the skull.",
        "The solution is not secondary.",
      },
      new Dictionary<string, string>()
    ),
    new TypeWriterPuzzleInstance(
      TypeWriterPuzzleID.One2Three,
      "ONE 2 THREE",
      new List<(SecretObjectiveID, string)> {
        (SecretObjectiveID.LookThroughWindow, "Get the detective to look out of the window for three consecutive seconds."),
        (SecretObjectiveID.DropCorrect, "Get the detective to drop the typewriter while the correct solution is written out (you must get them to do this before they hit 'submit')."),
        (SecretObjectiveID.SkullOffShelf, "Get the detective to throw the skull off of the bookshelf."),
      },
      new List<string> {
        "The solution might require a few hours' thought.",
        "The second word of the solution is a number.",
        "... Four!",
      },
      new Dictionary<string, string>()
    ),
    new TypeWriterPuzzleInstance(
      TypeWriterPuzzleID.FearOfElephants,
      "FEAR OF ELEPHANTS",
      new List<(SecretObjectiveID, string)> {
        (SecretObjectiveID.LookThroughWindow, "Get the detective to look out of the window for three consecutive seconds."),
        (SecretObjectiveID.SolveWithThreeOnTimer, "Get the detective to solve the puzzle while there is a '3' on the timer."),
        (SecretObjectiveID.TypeGIRAFFE, "Get the detective to type the word 'GIRAFFE'."),
      },
      new List<string> {
        "The solution is a feeling of immense apprehension. X = 4.",
        "Pachyderm = Elephant. Y = 2.",
        "Books 4 and 10 on the wall shelf are clues to the solution. Z = 9.",
      },
      new Dictionary<string, string>() {
        {"X", "4"},
        {"Y", "2"},
        {"Z", "9"},
      }
    ),
    new TypeWriterPuzzleInstance(
      TypeWriterPuzzleID.PlantsAndAnimals,
      "PLANTS AND ANIMALS",
      new List<(SecretObjectiveID, string)> {
        (SecretObjectiveID.SolveQuickly, "Ensure that the puzzle is solved with 3 or more minutes remaining."),
        (SecretObjectiveID.SetClockTo545, "Get the detective to set the grandfather clock's time to 5:45 (or later)."),
        (SecretObjectiveID.StationaryGramophone, "Make sure that the detective does NOT move the gramophone."),
      },
      new List<string> {
        "The solution can be seen high up in the night sky outside the window.",
        "There is a clue stuck to the bottom of the gramophone.",
        "There is a clue on the top of a bookshelf.",
      },
      new Dictionary<string, string>()
    ),
  };
  public List<SecretObjective> currentSecretObjectives;
  public List<string> currentClues;
  private bool _readyToSetUpLevel;
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
    EventManager.AddListener<GameEndEvent>(onGameEnd);
    LoadPrefereces();
  }

  void LoadPrefereces()
  {
    GamePreferences.Load();
    minutesPerRound = GamePreferences.MinutesPerRound;
  }

  // Start is called before the first frame update
  void Start()
  {
    PlayerManager = PlayerManager.Instance;
    _readyToSetUpLevel = true;
  }

  private void OnDestroy()
  {
    EventManager.RemoveListener<LevelStartEvent>(onGameStart);
    EventManager.RemoveListener<LevelEndEvent>(onLevelEnd);
    EventManager.RemoveListener<GameEndEvent>(onGameEnd);
  }

  public int getCurrentDetective()
  {
    return detectiveOrder[currentRound];
  }

  public TypeWriterPuzzleInstance getCurrentPuzzle()
  {
    return puzzles[currentRound];
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
    GamePreferences.MinutesPerRound = minutesPerRound;
    GamePreferences.Save();
  }

  public void updateMouseSensitivity(float value)
  {
    mouseSensitivity = value;
  }

    public void updateObjectRotationSpeed(float value)
    {
        objectRotationSpeed = value;
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
    if (_readyToSetUpLevel)
    {
      // FOR DEV PURPOSES: (REMOVE ON BUILD)
      PlayerManager.fillPlayers();
      // ^
      currentRound++; // Must be done FIRST
      assignSecretObjectives();
      LevelSetupCompleteEvent e = new LevelSetupCompleteEvent();
      EventManager.Broadcast(e);
      _readyToSetUpLevel = false;
    }
  }

  void onLevelEnd(LevelEndEvent e)
  {
    _readyToSetUpLevel = true;
    _roundEndText = e.endMessage;
  }

  public void LoadGameEndScene()
  {
    Debug.Log("Game End");
    LoadScene("GameEnd");
  }
  void onGameEnd(GameEndEvent e)
  {
    _gameEndText = e.endMessage;
  }
}
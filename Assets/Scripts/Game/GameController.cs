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
    currentRound = 0;
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
    currentSecretObjectives = new List<SecretObjective>();
    currentClues = new List<string>();
    List<int> order = getRandomSOAssignment();
    // Hardcoded for now. LATER: secret objectives depend on the puzzle instance
    SecretObjective so1 = new SecretObjective(
        PlayerManager.getPlayerByID(order[0]),
        "Get the detective to look out of the window for three consecutive seconds.",
        "The answer is in alphabetical order.",
        SecretObjectiveID.LookThroughWindow
    );
    currentSecretObjectives.Add(so1);
    currentClues.Add("");

    SecretObjective so2 = new SecretObjective(
        PlayerManager.getPlayerByID(order[1]),
        "Get the detective to turn the typewriter upside-down.",
        "The answer is very colourful.",
        SecretObjectiveID.InvertTypewriter
    );
    currentSecretObjectives.Add(so2);

    SecretObjective so3 = new SecretObjective(
        PlayerManager.getPlayerByID(order[2]),
        "Get the detective to type 'FOOL' into the typewriter.",
        "The answer is not secondary.",
        SecretObjectiveID.TypeFOOL
    );
    currentSecretObjectives.Add(so3);
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
    LoadScene("End");
  }

  private bool checkCanStartGame()
  {
    return forceStart || PlayerManager.players.Count == 4;
  }
  public void LoadLevel()
  {
    if (checkCanStartGame())
    {
      currentRound++;
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
    assignSecretObjectives();
    LevelSetupCompleteEvent e = new LevelSetupCompleteEvent();
    EventManager.Broadcast(e);
  }

  void onLevelEnd(LevelEndEvent e)
  {
    _roundEndText = e.endMessage;
  }
}
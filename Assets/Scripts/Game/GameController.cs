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
  public List<int> currentSecretObjectiveAssignment = new List<int>();
  public int minutesPerRound = 3;
  void Awake()
  {
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
    randomizeSecretObjectives();

    currentRound = 0;

    EventManager.AddListener<LevelStartEvent>(onGameStart);
    EventManager.AddListener<LevelEndEvent>(onLevelEnd);

    if (Instance != null && Instance != this)
    {
      Destroy(this);
    }
    else
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
  }

  void randomizeSecretObjectives()
  {
    // Randomize
    List<int> informants = new List<int> { 0, 1, 2, 3 };
    // remove the current detective
    informants.Remove(detectiveOrder[currentRound]);
    for (int i = 0; i < 3; i++)
    {
      int index = Mathf.FloorToInt(Random.Range(0, informants.Count));
      currentSecretObjectiveAssignment.Add(informants[index]);
      informants.Remove(informants[index]);
    }
  }

  // Start is called before the first frame update
  void Start()
  {
    PlayerManager = PlayerManager.Instance;
  }

  // Update is called once per frame
  void Update()
  {
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
      // FOR DEV PURPOSES: (REMOVE ON BUILD)
      PlayerManager.fillPlayers();
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
    LevelSetupCompleteEvent e = new LevelSetupCompleteEvent();
    EventManager.Broadcast(e);
  }

  void onLevelEnd(LevelEndEvent e)
  {
    _roundEndText = e.endMessage;
  }
}
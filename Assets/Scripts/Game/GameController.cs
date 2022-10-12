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

  public List<Instructor> instructors = new List<Instructor>();

  public string _roundEndText;
  void Awake()
  {
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

  // Start is called before the first frame update
  void Start()
  {
    PlayerManager = PlayerManager.Instance;
  }

  // Update is called once per frame
  void Update()
  {
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
      LoadScene("_MAINSCENE");
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

    foreach (var instructor in instructors)
    {
      levelManager.AddInstructors(instructor);
    }
  }

  public void SetupLevel()
  {
    instructors.Add(new Instructor("Instructor 0", TypeWriterSecretGoals.TypedFool, "The answer is very colourful."));
    instructors.Add(new Instructor("Instructor 1", TypeWriterSecretGoals.FlippedTypeWriter, "The answer is not secondary."));
    instructors.Add(new Instructor("Instructor 2", GeneralSecretGoals.LookThroughWindow, "The answer is in alphabetical order."));

    LevelSetupCompleteEvent e = new LevelSetupCompleteEvent();
    EventManager.Broadcast(e);
  }

  void onLevelEnd(LevelEndEvent e)
  {
    _roundEndText = e.endMessage;
  }
}
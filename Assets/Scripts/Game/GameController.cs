using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public PlayerManager PlayerManager;
    public PuzzleManager PuzzleManager;

    void Awake()
    {
        EventManager.AddListener<GameStartEvent>(onGameStart);

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
        return PlayerManager.players.Count == 4;
    }
    public void LoadPuzzle()
    {
        if (checkCanStartGame())
        {
            LoadScene("Puzzle");
        } else
        {
            Debug.Log("Not enough players");
        }
    }

    public bool IsGameEnding()
    {
        if (PuzzleManager == null)
        {
            return false;
        }
        else
        {
            return PuzzleManager.gameIsEnding;
        }
    }

    private void onGameStart(GameStartEvent e)
    {
        Debug.Log("Game Start received by GameController");
        PuzzleManager = GameObject.Find("PuzzleManager").GetComponent<PuzzleManager>();
    }
}
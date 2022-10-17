using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;


public class LevelManager : MonoBehaviour
{
    [Header("Parameters")] [Tooltip("Duration of the fade-to-black at the end of the game")]
    public float endSceneLoadDelay = 3f;

    [Tooltip("The canvas group of the fade-to-black screen")]
    public CanvasGroup endGameFadeCanvasGroup;

    [Header("Ending")] [Tooltip("This string has to be the name of the scene you want to load when game ends")]
    public string endSceneName = "End";

    public float puzzleTime = 60f * 3f;
    [SerializeField] private float _timeLeft = 0f;
    [SerializeField] private bool puzzleStarted;
    private UIController uiController;
    public AudioController audioController;


    public List<Informant> informants = new List<Informant> { };
    public bool gameIsEnding;
    private float _timeLoadEndGameScene;

    public List<Puzzle> puzzles = new List<Puzzle> { };

    void Start()
    {
        _timeLeft = puzzleTime; //3f;

        puzzleStarted = true;
        audioController = GameObject.Find("AudioManager").GetComponent<AudioController>();
        uiController = GameObject.Find("Hud").GetComponent<UIController>();

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

    public void AddInformants(Informant informant)
    {
        informants.Add(informant);
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
        string roundEndText = "In this round, ";
        foreach (var puzzle in puzzles)
        {
            if (puzzle.isComplete)
            {
                roundEndText += $"Puzzle {puzzle.name} is complete. \n";
            }
            else
            {
                roundEndText += $"Puzzle {puzzle.name} is not complete. \n";
            }
        }

        foreach (var informant in informants)
        {
            bool flag = false;
            foreach (var puzzle in puzzles)
            {
                if (informant.CheckSecretGoal(puzzle))
                {
                    roundEndText += $"{informant.name} completed secret goal '{informant._goal.description} '\n";
                    flag = true;
                }
            }

            if (!flag)
            {
                roundEndText += $"{informant.name}'s secret goal '{informant._goal.description}' was not complete. \n";
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
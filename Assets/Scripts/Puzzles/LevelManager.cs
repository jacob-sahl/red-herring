using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class LevelManager : MonoBehaviour
{
    [Header("Parameters")] [Tooltip("Duration of the fade-to-black at the end of the game")]
    public float endSceneLoadDelay = 3f;

    [Tooltip("The canvas group of the fade-to-black screen")]
    public CanvasGroup endGameFadeCanvasGroup;

    [Header("Ending")] [Tooltip("This string has to be the name of the scene you want to load when game ends")]
    public string endSceneName = "EndScene";

    public float puzzleTime = 60f * 3f;
    [SerializeField] private float _timeLeft = 0f;
    [SerializeField] private bool puzzleStarted;
    private UIController uiController;
    public AudioController audioController;


    public List<Instructor> instructors = new List<Instructor> { };
    public bool gameIsEnding;
    private float _timeLoadEndGameScene;

    public List<Puzzle> puzzles = new List<Puzzle> { };

    void Start()
    {
        _timeLeft = puzzleTime;

        puzzleStarted = true;
        audioController = GameObject.Find("AudioManager").GetComponent<AudioController>();
        uiController = GameObject.Find("Hud").GetComponent<UIController>();

        LevelStartEvent levelStartEvent = new LevelStartEvent();
        EventManager.Broadcast(levelStartEvent);
        Debug.Log("GameStartEvent broadcasted");
        
        SetupInstructors();
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
                Time.timeScale = 0;
                // ends the game
                EndLevel();
            }

            _timeLeft = puzzleTime;
            puzzleStarted = true;
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

    private void SetupInstructors()
    {
        instructors[0].name = "Instructor 0";
        instructors[1].name = "Instructor 1";
        instructors[2].name = "Instructor 2";
        this.instructors[0].SetupSecretGoal(TypeWriterSecretGoals.TypedFool.goal);
        this.instructors[1].SetupSecretGoal(GeneralSecretGoals.LookThroughWindow.goal);
        this.instructors[2].SetupSecretGoal(TypeWriterSecretGoals.FlippedTypeWriter.goal);
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
        foreach (var puzzle in puzzles)
        {
            if (puzzle.isComplete)
            {
                Debug.Log($"Puzzle {puzzle.name} is complete");
            }
            else
            {
                Debug.Log($"Puzzle {puzzle.name} is not complete");
            }
        }

        foreach (var instructor in instructors)
        {
            foreach (var puzzle in puzzles)
            {
                if (instructor.CheckSecretGoal(puzzle))
                {
                    Debug.Log($"{instructor.name}'s secret goal: True");
                }
            }
        }
        
        
        LevelEndEvent levelEndEvent = new LevelEndEvent();
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
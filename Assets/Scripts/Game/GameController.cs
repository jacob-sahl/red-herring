using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameController : MonoBehaviour
{
    [FormerlySerializedAs("EndSceneLoadDelay")] [Header("Parameters")] [Tooltip("Duration of the fade-to-black at the end of the game")]
    public float endSceneLoadDelay = 3f;

    [FormerlySerializedAs("EndGameFadeCanvasGroup")] [Tooltip("The canvas group of the fade-to-black screen")]
    public CanvasGroup endGameFadeCanvasGroup;

    [FormerlySerializedAs("EndSceneName")] [Header("Ending")] [Tooltip("This string has to be the name of the scene you want to load when game ends")]
    public string endSceneName = "EndScene";
    
    public bool gameIsEnding;

    private float _timeLoadEndGameScene;

    void Awake()
    {
        EventManager.AddListener<GameOverEvent>(OnGameOver);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsEnding)
        {
            float timeRatio = 1 - (_timeLoadEndGameScene - Time.time) / endSceneLoadDelay;
            endGameFadeCanvasGroup.alpha = timeRatio;

            if (Time.time >= _timeLoadEndGameScene)
            {
                // SceneManager.LoadScene(endSceneName);
                gameIsEnding = false;
            }
        }
    }

    void OnGameOver(GameOverEvent evt) => EndGame(evt.PuzzleSolved, evt.EndGameMessage);

    void EndGame(bool puzzleSolved, string endGameMessage)
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
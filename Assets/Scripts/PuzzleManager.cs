using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonType
{
    Key1,
    Key2,
    Key3,
    Key4,
    Reset,
    Green
}

public class PuzzleManager : MonoBehaviour
{
    
    public float puzzleTime = 60f * 3f;
    [SerializeField] private float _timeLeft = 0f;
    [SerializeField] private bool puzzleStarted;
    private UIController uiController;
    
    [SerializeField] private List<ButtonType> _pressed = new List<ButtonType>{};
    
    [SerializeField] private List<ButtonType> _solution = new List<ButtonType>{ ButtonType.Key2, ButtonType.Key3, ButtonType.Key4, ButtonType.Key1, ButtonType.Key4, ButtonType.Key2, ButtonType.Key2 };
    [SerializeField] private List<ButtonType> _answer = new List<ButtonType>{};
    
    public List<Instructor> instructors = new List<Instructor>{};
    // Start is called before the first frame update
    void Start()
    {
        SetupInstructors();

        _timeLeft = puzzleTime;
        puzzleStarted = true;
        uiController = GameObject.Find("Canvas").GetComponent<UIController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (puzzleStarted) {
            _timeLeft -= Time.deltaTime;
            uiController.displayTime(_timeLeft);
        
            if (_timeLeft <= 0)
            {
                Time.timeScale = 0;
                // ends the game
                EndPuzzle();
            }
        }
        
    }

    public void ButtonPressed(ButtonType button)
    {
        _pressed.Add(button);
        switch (button)
        {
            case ButtonType.Key1:
            case ButtonType.Key2:
            case ButtonType.Key3:
            case ButtonType.Key4:
                _answer.Add(button);
                if (CheckAnswer())
                {
                    EndPuzzle();
                };
                break;
            case ButtonType.Reset:
                _answer.Clear();
                break;
            case ButtonType.Green:
                break;
        }

    }

    private bool CheckAnswer()
    {
        if (_answer.Count == _solution.Count)
        {
            for (int i = 0; i < _answer.Count; i++)
            {
                if (_answer[i] != _solution[i])
                {
                    return false;
                }
            }

            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetupInstructors()
    {
        this.instructors[0].name = "Instructor 0";
        this.instructors[1].name = "Instructor 1";
        this.instructors[2].name = "Instructor 2";
        this.instructors[0].SetupSecretGoal(pressed =>
        {
            if (pressed.Count > 4)
            {
                return pressed[2] == ButtonType.Reset && pressed[3] == ButtonType.Reset;
            }
            else
            {
                return false;
            }
        });
        
        this.instructors[1].SetupSecretGoal(pressed => pressed.Contains(ButtonType.Green));
        this.instructors[2].SetupSecretGoal(pressed => !pressed.Contains(ButtonType.Green));
    }

    private void EndPuzzle()
    {
        uiController.displayText("Game Over");
        Debug.Log($"Box Opened: {CheckAnswer()}");
        foreach (var instructor in instructors)
        {
            Debug.Log($"{instructor.name}'s secret goal: {instructor.CheckSecretGoal(_pressed)}");
        }

    }
}
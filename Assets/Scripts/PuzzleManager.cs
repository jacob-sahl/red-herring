using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Button
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
    
    public float puzzleTime = 60f * 5f;
    [SerializeField] private float _timeLeft = 0f;
    
    [SerializeField] private List<Button> _pressed = new List<Button>{};
    
    [SerializeField] private List<Button> _solution = new List<Button>{ Button.Key2, Button.Key3, Button.Key4, Button.Key1, Button.Key4, Button.Key2, Button.Key2 };
    [SerializeField] private List<Button> _answer = new List<Button>{};
    
    public List<Instructor> instructors = new List<Instructor>{};
    // Start is called before the first frame update
    void Start()
    {
        SetupInstructors();

        _timeLeft = puzzleTime;
    }

    // Update is called once per frame
    void Update()
    {
        _timeLeft -= Time.deltaTime;
        
        if (_timeLeft <= 0)
        {
            Time.timeScale = 0;
            // ends the game
            EndPuzzle();
        }
    }

    public void ButtonPressed(Button button)
    {
        _pressed.Add(button);
        switch (button)
        {
            case Button.Key1:
            case Button.Key2:
            case Button.Key3:
            case Button.Key4:
                _answer.Add(button);
                if (CheckAnswer())
                {
                    EndPuzzle();
                };
                break;
            case Button.Reset:
                _answer.Clear();
                break;
            case Button.Green:
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
                return pressed[2] == Button.Reset && pressed[3] == Button.Reset;
            }
            else
            {
                return false;
            }
        });
        
        this.instructors[1].SetupSecretGoal(pressed => pressed.Contains(Button.Green));
        this.instructors[2].SetupSecretGoal(pressed => !pressed.Contains(Button.Green));
    }

    private void EndPuzzle()
    {
        Debug.Log($"Box Opened: {CheckAnswer()}");
        foreach (var instructor in instructors)
        {
            Debug.Log($"{instructor.name}'s secret goal: {instructor.CheckSecretGoal(_pressed)}");
        }

    }
}
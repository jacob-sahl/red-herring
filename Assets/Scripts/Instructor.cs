using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructor : MonoBehaviour
{
    
    // an lambda function checks if wins
    public delegate bool secretGoal(List<Button> pressed);

    public new string name = "";
    private secretGoal _goal;

    public void SetupSecretGoal(secretGoal goal)
    {
        // do something with the goal
        this._goal = goal;
    }
    
    public bool CheckSecretGoal(List<Button> pressed)
    {
        // do something with the pressed buttons
        return _goal(pressed);
    }
    
    
}

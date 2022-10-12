using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructor : MonoBehaviour
{

  // an lambda function checks if wins
  public delegate bool secretGoal(Puzzle puzzle);
  public new string name = "";
  public Color color;
  private secretGoal _goal;

  public void SetupSecretGoal(secretGoal goal)
  {
    // do something with the goal
    this._goal = goal;
  }

  public bool CheckSecretGoal(Puzzle puzzle)
  {
    // do something with the pressed buttons
    return _goal(puzzle);
  }

}

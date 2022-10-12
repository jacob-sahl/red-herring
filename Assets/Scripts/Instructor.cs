using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructor
{

  // an lambda function checks if wins
  public delegate bool secretGoal(Puzzle puzzle);
  public new string name = "";
  private secretGoal _goal;

  public Instructor(string name, secretGoal goal)
  {
    this.name = name;
    this._goal = goal;
  }
  
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

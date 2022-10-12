using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructor
{

  // an lambda function checks if wins
  public new string name = "";
  private SecretGoal _goal;

  public Instructor(string name, SecretGoal goal)
  {
    this.name = name;
    this._goal = goal;
  }
  
  public void SetupSecretGoal(SecretGoal goal)
  {
    // do something with the goal
    this._goal = goal;
  }

  public bool CheckSecretGoal(Puzzle puzzle)
  {
    // do something with the pressed buttons
    return _goal.checkSecretGoal(puzzle);
  }

}

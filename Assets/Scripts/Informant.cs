using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Informant
{

  // an lambda function checks if wins
  public new string name = "";
  public SecretGoal _goal;
  public string clue;

  public Informant(string name, SecretGoal goal, string clue)
  {
    this.name = name;
    this._goal = goal;
    this.clue = clue;
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

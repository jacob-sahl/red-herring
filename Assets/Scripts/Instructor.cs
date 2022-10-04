using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructor : MonoBehaviour
{

  // an lambda function checks if wins
  public delegate bool secretGoal(List<ButtonType> pressed);
  public new string name = "";
  public Color color;
  private secretGoal _goal;

  public void SetupSecretGoal(secretGoal goal)
  {
    // do something with the goal
    this._goal = goal;
  }

  public bool CheckSecretGoal(List<ButtonType> pressed)
  {
    // do something with the pressed buttons
    return _goal(pressed);
  }


}

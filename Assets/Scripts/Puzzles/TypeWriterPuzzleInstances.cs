using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TypeWriterPuzzleInstance
{

  public string solution;
  public List<(SecretObjectiveID, string)> secrets;
  public List<string> clues;

  public TypeWriterPuzzleInstance(string solution, List<(SecretObjectiveID, string)> secrets, List<string> clues)
  {
    this.solution = solution;
    this.clues = clues;
    this.secrets = secrets;
  }

}
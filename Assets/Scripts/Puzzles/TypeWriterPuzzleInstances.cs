using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeWriterPuzzleID
{
  BlueRedYellow,
  One2Three,
  FearOfElephants,
  PlantsAndAnimals,
}

public class TypeWriterPuzzleInstance
{
  public TypeWriterPuzzleID id;
  public string solution;
  public List<(SecretObjectiveID, string)> secrets;
  public List<string> clues;
  public Dictionary<string, string> query;

  public TypeWriterPuzzleInstance(
    TypeWriterPuzzleID id,
    string solution,
    List<(SecretObjectiveID, string)> secrets,
    List<string> clues,
    Dictionary<string, string> query
  )
  {
    this.id = id;
    this.solution = solution;
    this.clues = clues;
    this.secrets = secrets;
    this.query = query;
  }
}
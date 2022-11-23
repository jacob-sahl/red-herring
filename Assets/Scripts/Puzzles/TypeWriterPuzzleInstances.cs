using System.Collections.Generic;

public enum TypeWriterPuzzleID
{
    BlueRedYellow,
    One2Three,
    FearOfElephants,
    PlantsAndAnimals
}

public class TypeWriterPuzzleInstance
{
    public List<string> clues;
    public TypeWriterPuzzleID id;
    public Dictionary<string, string> query;
    public List<(SecretObjectiveID, string)> secrets;
    public string solution;

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
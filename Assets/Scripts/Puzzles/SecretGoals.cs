using UnityEngine;

public static class GeneralSecretGoals
{
    public static SecretGoal LookThroughWindow = new SecretGoal((Puzzle puzzle) => true,
        "Get the detective to look out of a window for three consecutive seconds.");
}

public struct SecretGoal
{
    public Instructor.secretGoal goal;
    public string description;

    public SecretGoal(Instructor.secretGoal goal, string description)
    {
        this.goal = goal;
        this.description = description;
    }
}
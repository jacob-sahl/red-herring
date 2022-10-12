using UnityEngine;

public delegate bool SecretGoalDelegate(Puzzle puzzle);

public static class GeneralSecretGoals
{
    public static SecretGoal LookThroughWindow = new SecretGoal((Puzzle puzzle) => true,
        "Get the detective to look out of a window for three consecutive seconds.");
}

public struct SecretGoal
{
    public SecretGoalDelegate checkSecretGoal;
    public string description;

    public SecretGoal(SecretGoalDelegate checkSecretGoal, string description)
    {
        this.checkSecretGoal = checkSecretGoal;
        this.description = description;
    }
}
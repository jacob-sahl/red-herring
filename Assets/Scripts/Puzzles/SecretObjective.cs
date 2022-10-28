using UnityEngine;

public enum SecretObjectiveID
{
  LookThroughWindow,
  TypeFOOL,
  InvertTypewriter,
  DropCorrect,
  SpinGlobeThrice,
  TypeFIVE,
  SkullOffShelf,
}

public class SecretObjective
{
  public SecretObjectiveID id;
  public PlayerController player;
  public string description;
  public string clue;
  public bool completed;

  public SecretObjective(PlayerController player, string desc, string clue, SecretObjectiveID id)
  {
    this.id = id;
    this.player = player;
    this.completed = false;
    this.description = desc;
    this.clue = clue;
    EventManager.AddListener<SecretObjectiveEvent>(updateStatus);
  }

  public void Deconstruct()
  {
    EventManager.RemoveListener<SecretObjectiveEvent>(updateStatus);
  }

  public void updateStatus(SecretObjectiveEvent evt)
  {
    if (evt.id == this.id)
    {
      this.completed = evt.status;
      // TODO: rumble the controller of the informant
      Debug.Log("Secret objective updated: " + description);
      Debug.Log("Status: " + completed);
    }
  }
}
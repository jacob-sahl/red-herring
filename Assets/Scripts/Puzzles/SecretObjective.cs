using UnityEngine;

public enum SecretObjectiveID
{
  LookThroughWindow,
  TypeFOOL,
  InvertTypewriter
}

public class SecretObjective
{
  public SecretObjectiveID id;
  public PlayerController player;
  public string description;
  public bool completed;

  public SecretObjective(PlayerController player, string desc, SecretObjectiveID id)
  {
    this.id = id;
    this.player = player;
    this.completed = false;
    this.description = desc;
    EventManager.AddListener<SecretObjectiveEvent>(updateStatus);
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
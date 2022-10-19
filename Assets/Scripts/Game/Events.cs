using UnityEngine;

public static class Events
{
  public static LevelEndEvent levelEndEvent = new LevelEndEvent();
  public static LevelStartEvent LevelStartEvent = new LevelStartEvent();
  public static LevelSetupCompleteEvent LevelSetupCompleteEvent = new LevelSetupCompleteEvent();
  public static DisplayMessageEvent DisplayMessageEvent = new DisplayMessageEvent();
  public static InteractEvent InteractEvent = new InteractEvent();
  public static ICursorHoverEvent ICursorHoverEvent = new ICursorHoverEvent();
  public static FocusEvent FocusEvent = new FocusEvent();
  public static PlayerJoinedEvent PlayerJoinedEvent = new PlayerJoinedEvent();
  public static PlayerUpdateEvent PlayerUpdateEvent = new PlayerUpdateEvent();
  public static SecretObjectiveEvent SecretObjectiveEvent = new SecretObjectiveEvent();
}


public class LevelEndEvent : GameEvent
{
  public string endMessage;
}

public class DisplayMessageEvent : GameEvent
{
  public string Message;
  public float DelayBeforeDisplay;
}

public class InteractEvent : GameEvent
{
  public GameObject gameObject;
}

public class LookEvent : GameEvent
{
  public GameObject gameObject;
}

public class LevelStartEvent : GameEvent
{
}

public class ICursorHoverEvent : GameEvent
{
  public string ObjectTag;
}

public class FocusEvent : GameEvent
{
  public string ObjectTag;
}

public class PlayerJoinedEvent : GameEvent
{
  public int PlayerID;
}

public class PlayerUpdateEvent : GameEvent
{
  public int PlayerID;
}

public class LevelSetupCompleteEvent : GameEvent
{
  public int PlayerID;
}

public class SecretObjectiveEvent : GameEvent
{
  public SecretObjectiveID id;
  public bool status;
}

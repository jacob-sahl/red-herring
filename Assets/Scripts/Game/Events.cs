using System.Collections.Generic;
using APIClient;
using UnityEngine;

public static class Events
{
  public static LevelEndEvent levelEndEvent = new();
  public static LevelStartEvent LevelStartEvent = new();
  public static LevelSetupCompleteEvent LevelSetupCompleteEvent = new();
  public static DisplayMessageEvent DisplayMessageEvent = new();
  public static InteractEvent InteractEvent = new();
  public static ICursorHoverEvent ICursorHoverEvent = new();
  public static FocusEvent FocusEvent = new();
  public static DefocusEvent DefocusEvent = new();
  public static PlayerJoinedEvent PlayerJoinedEvent = new();
  public static PlayerUpdateEvent PlayerUpdateEvent = new();
  public static SecretObjectiveEvent SecretObjectiveEvent = new();
  public static ClockTimeChangeEvent ClockTimeChangeEvent = new();
  public static GamePreferenceChangeEvent GamePreferenceChangeEvent = new();
  public static GameEndEvent GameEndEvent = new();
  public static GameCreatedEvent GameCreatedEvent = new();
  public static GameInstanceUpdatedEvent GameInstanceUpdatedEvent = new();
  public static UIAnimationEndEvent UIAnimationEndEvent = new UIAnimationEndEvent();
  public static UIAnimationStartEvent UIAnimationStartEvent = new UIAnimationStartEvent();
  public static UIAnimationInterruptAllEvent UIAnimationInterruptAllEvent = new UIAnimationInterruptAllEvent();
}

public class GameCreatedEvent : GameEvent
{
  public GameInstance gameInstance;
}

public class LevelEndEvent : GameEvent
{
  public bool puzzleCompleted;
  public List<string> messages;
  public List<List<int>> pointStages;
}

public class DisplayMessageEvent : GameEvent
{
  public float DelayBeforeDisplay;
  public string Message;
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
  public GameObject gameObject;
}

public class DefocusEvent : GameEvent
{
  public GameObject gameObject;
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

public class ClockTimeChangeEvent : GameEvent
{
  public int minutes;
}

public class GamePreferenceChangeEvent : GameEvent
{
}

public class GameEndEvent : GameEvent
{
  public string endMessage;
}

public class GameInstanceUpdatedEvent : GameEvent
{
  public GameInstance gameInstance;
}
public class UIAnimationStartEvent : GameEvent
{
  public string name;
}

public class UIAnimationEndEvent : GameEvent
{
  public string name;
}

public class UIAnimationInterruptAllEvent : GameEvent
{
  public string name;
}

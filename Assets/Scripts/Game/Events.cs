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
  public static DefocusEvent DefocusEvent = new DefocusEvent();
  public static PlayerJoinedEvent PlayerJoinedEvent = new PlayerJoinedEvent();
  public static PlayerUpdateEvent PlayerUpdateEvent = new PlayerUpdateEvent();
  public static SecretObjectiveEvent SecretObjectiveEvent = new SecretObjectiveEvent();
  public static ClockTimeChangeEvent ClockTimeChangeEvent = new ClockTimeChangeEvent();
  public static GamePreferenceChangeEvent GamePreferenceChangeEvent = new GamePreferenceChangeEvent();
  public static GameEndEvent GameEndEvent = new GameEndEvent();
  public static UIAnimationEndEvent UIAnimationEndEvent = new UIAnimationEndEvent();
  public static UIAnimationStartEvent UIAnimationStartEvent = new UIAnimationStartEvent();
  public static UIAnimationInterruptAllEvent UIAnimationInterruptAllEvent = new UIAnimationInterruptAllEvent();
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

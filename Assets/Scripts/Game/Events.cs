using UnityEngine;

public static class Events
{
  public static GameOverEvent GameOverEvent = new GameOverEvent();
  public static DisplayMessageEvent DisplayMessageEvent = new DisplayMessageEvent();
  public static InteractEvent InteractEvent = new InteractEvent();
  public static ICursorHoverEvent ICursorHoverEvent = new ICursorHoverEvent();
  public static FocusEvent FocusEvent = new FocusEvent();
}


public class GameOverEvent : GameEvent
{
  public bool PuzzleSolved;
  public string EndGameMessage;
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

public class ICursorHoverEvent : GameEvent
{
  public string ObjectTag;
}

public class FocusEvent : GameEvent
{
  public string ObjectTag;
}
using System.Collections.Generic;

public static class Constants
{
  public const string AxisNameVertical = "Vertical";
  public const string AxisNameHorizontal = "Horizontal";
  public const string MouseAxisNameVertical = "Mouse Y";
  public const string MouseAxisNameHorizontal = "Mouse X";
  public const string AxisNameJoystickLookVertical = "Look Y";
  public const string AxisNameJoystickLookHorizontal = "Look X";

  public const string ButtonNameInteract = "Interact";

  public static Dictionary<string, string> keyboardControls = new Dictionary<string, string>
  {
    {"Look", "Mouse"},
    {"Movement", "WASD / Arrow Keys"},
    {"Interact", "Left Click"},
    {"Unfocus Object", "E"},
    {"Crouch", "Ctrl"},
    {"Jump", "Space"},
    {"Pause", "Tab"}
  };
  public static Dictionary<string, string> gamepadControls = new Dictionary<string, string>
  {
    {"Look", "Right Stick"},
    {"Movement", "Left Stick"},
    {"Interact", "West Button ( X / □ )"},
    {"Unfocus Object", "East Button ( B / O )"},
    {"Crouch", "Press Left Stick"},
    {"Jump", "South Button ( A / X )"},
    {"Pause", "Start"}
  };
}
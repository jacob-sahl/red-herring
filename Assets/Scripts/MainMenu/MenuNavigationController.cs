using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNavigationController : MonoBehaviour
{
  MenuAnimationController animationController;
  private int position;
  private bool navEnabled;
  private string waitingFor;
  private void Awake()
  {
    EventManager.AddListener<UIAnimationEndEvent>(onAnimationFinished);
  }

  private void OnDestroy()
  {
    EventManager.RemoveListener<UIAnimationEndEvent>(onAnimationFinished);
  }
  void Start()
  {
    navEnabled = true;
    position = 0;
    animationController = GameObject.Find("MenuAnimator").GetComponent<MenuAnimationController>();
  }

  void onAnimationFinished(UIAnimationEndEvent e)
  {
    if (e.name == waitingFor)
    {
      waitingFor = "";
      navEnabled = true;
    }
  }

  public void moveForward()
  {
    if (!navEnabled) return;
    List<string> animations = new List<string>();
    switch (position)
    {
      case 0:
        animations = new List<string> { "textHide1-1", "textReveal1-2" };
        break;
      case 1:
        animations = new List<string> {
          "fadeOutWelcome", "textHide1-2", "fadeInD&I", "textReveal2-1"
        };
        break;
      case 2:
        animations = new List<string> { "textHide2-1", "textReveal2-2" };
        break;
      case 3:
        animations = new List<string> { "textHide2-2", "textReveal2-3" };
        break;
      case 4:
        animations = new List<string> { "textHide2-3", "textReveal2-4" };
        break;
      case 5:
        animations = new List<string> { "textHide2-4", "textReveal2-5" };
        break;
      case 6:
        animations = new List<string> {
          "fadeOutD&I", "textHide2-5", "fadeInPuzzle", "textReveal3-1"
        };
        break;
      case 7:
        animations = new List<string> { "textHide3-1", "textReveal3-2" };
        break;
      case 8:
        animations = new List<string> { "textHide3-2", "textReveal3-3" };
        break;
      case 9:
        animations = new List<string> {
          "fadeOutPuzzle", "textHide3-3", "fadeInSO", "textReveal4-1"
        };
        break;
      case 10:
        animations = new List<string> { "textHide4-1", "textReveal4-2" };
        break;
      case 11:
        animations = new List<string> { "textHide4-2", "textReveal4-3" };
        break;
      case 12:
        animations = new List<string> { "textHide4-3", "textReveal4-4" };
        break;
      default:
        return;
    }

    animationController.startAnimationGroup(animations);

    // Wait for the last animation to finish before allowing more navigation
    waitingFor = animations[animations.Count - 1];
    navEnabled = false;

    position++;
  }

  public void moveBackward()
  {
    if (!navEnabled) return;
    List<string> animations = new List<string>();
    switch (position)
    {
      case 0:
        Debug.Log("Menu Nav: Cannot move backwards, position is 0");
        return;
      case 1:
        animations = new List<string> { "textHide1-2", "textReveal1-1" };
        break;
      case 2:
        animations = new List<string> {
          "fadeOutD&I", "textHide2-1", "fadeInWelcome", "textReveal1-2"
          };
        break;
      case 3:
        animations = new List<string> { "textHide2-2", "textReveal2-1" };
        break;
      case 4:
        animations = new List<string> { "textHide2-3", "textReveal2-2" };
        break;
      case 5:
        animations = new List<string> { "textHide2-4", "textReveal2-3" };
        break;
      case 6:
        animations = new List<string> { "textHide2-5", "textReveal2-4" };
        break;
      case 7:
        animations = new List<string> {
          "fadeOutPuzzle", "textHide3-1", "fadeInD&I", "textReveal2-5"
          };
        break;
      case 8:
        animations = new List<string> { "textHide3-2", "textReveal3-1" };
        break;
      case 9:
        animations = new List<string> { "textHide3-3", "textReveal3-2" };
        break;
      case 10:
        animations = new List<string> {
          "fadeOutSO", "textHide4-1", "fadeInPuzzle", "textReveal3-3"
          };
        break;
      case 11:
        animations = new List<string> { "textHide4-2", "textReveal4-1" };
        break;
      case 12:
        animations = new List<string> { "textHide4-3", "textReveal4-2" };
        break;
      case 13:
        animations = new List<string> { "textHide4-4", "textReveal4-3" };
        break;
    }

    animationController.startAnimationGroup(animations);

    // Wait for the last animation to finish before allowing more navigation
    waitingFor = animations[animations.Count - 1];
    navEnabled = false;

    position--;
  }
}

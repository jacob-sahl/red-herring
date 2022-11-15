using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MenuNavigationController : MonoBehaviour
{
  public EventSystem eventSystem;
  public GameObject mainMenuObject;
  public GameObject puzzleSetupObject;
  public GameObject objectToSelectOnPuzzleSetup;
  public GameObject nextButtonText;
  public GameObject skipButtonObject;
  Button skipButton;
  public GameObject menuButtonObject;
  Button menuButton;
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
    navEnabled = false;
    position = 0;
    animationController = GameObject.Find("MenuAnimator").GetComponent<MenuAnimationController>();
    skipButton = skipButtonObject.GetComponent<Button>();
    skipButton.interactable = false;
    menuButton = menuButtonObject.GetComponent<Button>();
    menuButton.interactable = false;
    waitingFor = "textReveal1-1";
  }

  void onAnimationFinished(UIAnimationEndEvent e)
  {
    if (e.name == waitingFor)
    {
      skipButton.interactable = true;
      menuButton.interactable = true;
      waitingFor = "";
      navEnabled = true;
    }
  }

  public void moveForward()
  {
    if (!navEnabled)
    {
      UIAnimationInterruptAllEvent evt = new UIAnimationInterruptAllEvent();
      evt.name = "test";
      EventManager.Broadcast(evt);
      return;
    }
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
          "fadeOutD&I", "textHide2-5", "fadeInPuzzle","typewriterImageFadeIn", "textReveal3-1"
        };
        break;
      case 7:
        animations = new List<string> { "typewriterImageFadeOut", "textHide3-1", "hintImageFadeIn", "textReveal3-2" };
        break;
      case 8:
        animations = new List<string> { "hintImageFadeOut", "textHide3-2", "clueImageFadeIn", "textReveal3-3" };
        break;
      case 9:
        animations = new List<string> {
          "clueImageFadeOut", "fadeOutPuzzle", "textHide3-3", "fadeInSO", "fullCardImageFadeIn", "textReveal4-1"
        };
        break;
      case 10:
        animations = new List<string> { "fullCardImageFadeOut", "textHide4-1", "textReveal4-2" };
        break;
      case 11:
        animations = new List<string> { "textHide4-2", "textReveal4-3" };
        break;
      case 12:
        animations = new List<string> { "textHide4-3", "textReveal4-4" };
        break;
      case 13:
        animations = new List<string> { "textHide4-4", "textReveal4-5" };
        break;
      case 14:
        animations = new List<string> { "textHide4-5", "textReveal4-6" };
        nextButtonText.GetComponent<TextMeshProUGUI>().text = "Begin";
        break;
      case 15:
        animations = new List<string> { "textHide4-6" };
        mainMenuObject.SetActive(false);
        puzzleSetupObject.SetActive(true);
        eventSystem.SetSelectedGameObject(objectToSelectOnPuzzleSetup);
        resetIntro();
        break;
      default:
        return;
    }

    animationController.startAnimationGroup(animations);

    // Wait for the last animation to finish before allowing more navigation
    waitingFor = animations[animations.Count - 1];
    navEnabled = false;
    skipButton.interactable = false;
    menuButton.interactable = false;

    position++;
  }

  public void moveBackward()
  {
    if (!navEnabled)
    {
      Debug.Log("Blocked. Waiting for: " + waitingFor);
      UIAnimationInterruptAllEvent evt = new UIAnimationInterruptAllEvent();
      evt.name = "test";
      EventManager.Broadcast(evt);
      return;
    }
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
          "typewriterImageFadeOut", "fadeOutPuzzle", "textHide3-1", "fadeInD&I", "textReveal2-5"
          };
        break;
      case 8:
        animations = new List<string> { "hintImageFadeOut", "textHide3-2", "typewriterImageFadeIn", "textReveal3-1" };
        break;
      case 9:
        animations = new List<string> { "clueImageFadeOut", "textHide3-3", "hintImageFadeIn", "textReveal3-2" };
        break;
      case 10:
        animations = new List<string> {
          "fullCardImageFadeOut", "fadeOutSO", "textHide4-1", "fadeInPuzzle", "clueImageFadeIn", "textReveal3-3"
          };
        break;
      case 11:
        animations = new List<string> { "textHide4-2", "fullCardImageFadeIn", "textReveal4-1" };
        break;
      case 12:
        animations = new List<string> { "textHide4-3", "textReveal4-2" };
        break;
      case 13:
        animations = new List<string> { "textHide4-4", "textReveal4-3" };
        break;
      case 14:
        animations = new List<string> { "textHide4-5", "textReveal4-4" };
        break;
      case 15:
        animations = new List<string> { "textHide4-6", "textReveal4-5" };
        nextButtonText.GetComponent<TextMeshProUGUI>().text = "Next";
        break;
    }

    animationController.startAnimationGroup(animations);

    // Wait for the last animation to finish before allowing more navigation
    waitingFor = animations[animations.Count - 1];
    navEnabled = false;
    skipButton.interactable = false;
    menuButton.interactable = false;

    position--;
  }

  public void resetIntro()
  {
    resetPosition();
    nextButtonText.GetComponent<TextMeshProUGUI>().text = "Next";
    waitingFor = "textReveal1-1";
    navEnabled = false;
    animationController.resetAllIntroContent();
  }

  public void resetPosition()
  {
    position = 0;
  }
}

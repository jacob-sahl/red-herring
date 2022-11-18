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
          "fadeOutWelcome", "textHide1-2", "fadeInRules", "textReveal2-1"
        };
        break;
      case 2:
        animations = new List<string> { "textHide2-1", "textReveal2-2" };
        break;
      case 3:
        animations = new List<string> { "textHide2-2", "textReveal2-3", "fullCardImageFadeIn" };
        break;
      case 4:
        animations = new List<string> { "fullCardImageFadeOut", "textHide2-3", "textReveal2-4" };
        break;
      case 5:
        animations = new List<string> { "textHide2-4", "textReveal2-5" };
        break;
      case 6:
        animations = new List<string> { "textHide2-5", "textReveal2-6" };
        break;
      case 7:
        animations = new List<string> { "textHide2-6", "textReveal2-7" };
        nextButtonText.GetComponent<TextMeshProUGUI>().text = "Begin";
        break;
      case 8:
        animations = new List<string> { "textHide2-7" };
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
          "fadeOutRules", "textHide2-1", "fadeInWelcome", "textReveal1-2"
          };
        break;
      case 3:
        animations = new List<string> { "textHide2-2", "textReveal2-1" };
        break;
      case 4:
        animations = new List<string> { "fullCardImageFadeOut", "textHide2-3", "textReveal2-2" };
        break;
      case 5:
        animations = new List<string> { "textHide2-4", "textReveal2-3", "fullCardImageFadeIn" };
        break;
      case 6:
        animations = new List<string> { "textHide2-5", "textReveal2-4" };
        break;
      case 7:
        animations = new List<string> { "textHide2-6", "textReveal2-5" };
        break;
      case 8:
        animations = new List<string> { "textHide2-7", "textReveal2-6" };
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

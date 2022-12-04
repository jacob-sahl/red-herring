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
  public VoiceOver _VO;
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
        _VO.playClip(1, delay: 0.25f, volume: 1f);
        break;
      case 1:
        animations = new List<string> { "textHide1-2", "playerTokensIn", "textReveal1-3" };
        _VO.playClip(2, delay: 1f, volume: 0.25f);
        break;
      case 2:
        animations = new List<string> { "textHide1-3", "textReveal1-4" };
        // Simultaneously ...
        animationController.startAnimation("P1raise");
        animationController.startAnimation("P2raise");
        animationController.startAnimation("P3raise");
        animationController.startAnimationGroup(new List<string> { "P4raise", "crownIn" });
        _VO.playClip(3, delay: 0.5f, volume: 0.25f);
        break;
      case 3:
        animations = new List<string> { "playerTokensOut", "textHide1-4", "detectiveGraphicIn", "textReveal1-5" };
        animationController.startAnimation("crownOut");
        _VO.playClip(4);
        break;
      case 4:
        animations = new List<string> { "detectiveGraphicOut", "textHide1-5", "textReveal1-6" };
        animationController.startAnimation("informantGraphicIn");
        _VO.playClip(5);
        break;
      case 5:
        animations = new List<string> { "textHide1-6", "textReveal1-7" };
        _VO.playClip(6, volume: 0.25f);
        break;
      case 6:
        animations = new List<string> { "informantGraphicOut", "textHide1-7", "textReveal1-8" };
        _VO.playClip(7);
        break;
      case 7:
        animations = new List<string> { "textHide1-8", "textReveal1-9" };
        nextButtonText.GetComponent<TextMeshProUGUI>().text = "Begin";
        _VO.playClip(8);
        break;
      case 8:
        animations = new List<string> { "textHide1-9" };
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
        animations = new List<string> { "textHide1-3", "textReveal1-2" };
        break;
      case 3:
        animations = new List<string> { "textHide1-4", "playerTokensIn", "textReveal1-3" };
        break;
      case 4:
        animations = new List<string> { "textHide1-5", "textReveal1-4" };
        break;
      case 5:
        animations = new List<string> { "textHide1-6", "textReveal1-5" };
        break;
      case 6:
        animations = new List<string> { "textHide1-7", "textReveal1-6" };
        break;
      case 7:
        animations = new List<string> { "textHide1-8", "textReveal1-7" };
        break;
      case 8:
        animations = new List<string> { "textHide1-9", "textReveal1-8" };
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

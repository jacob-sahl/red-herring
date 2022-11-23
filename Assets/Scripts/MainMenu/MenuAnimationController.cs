using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuAnimationController : MonoBehaviour
{
  private List<List<string>> animationGroups;
  void Awake()
  {
    EventManager.AddListener<UIAnimationEndEvent>(onAnimationFinished);
  }

  private void Start()
  {
    animationGroups = new List<List<string>>();
    startAnimation("mainLogoFadeIn");
    startAnimation("colourBackgroundFade");
  }
  private void OnDestroy()
  {
    EventManager.RemoveListener<UIAnimationEndEvent>(onAnimationFinished);
  }

  public void startAnimation(string animationName)
  {
    Debug.Log("Anim starting: " + animationName);
    UIAnimationStartEvent evt = new UIAnimationStartEvent();
    evt.name = animationName;
    EventManager.Broadcast(evt);
  }

  public void startAnimationGroup(List<string> animations)
  {
    animationGroups.Add(animations);
    startAnimation(animations[0]);
  }

  // BANDAID 
  // Ideally thse would be set in-editor but functions with List parameters don't show up in-editor
  public void playStartGameAnimation()
  {
    startAnimationGroup(new List<string> {
      "fadeMainMenuContentOut", "expandBackdrop", "fadeIntro1In", "fadeInWelcome", "textReveal1-1"
      });
  }

  public void returnToTitleScreen()
  {
    resetAllIntroContent();
    startAnimationGroup(new List<string> { "fadeIntro1Out", "collapseBackdrop", "fadeMainMenuContentIn" });
  }

  public void resetAllIntroContent()
  {
    List<string> animations = new List<string> {
      "textHide1-1instant",
      "textHide1-2instant",
      "textHide2-1instant",
      "textHide2-2instant",
      "textHide2-3instant",
      "textHide2-4instant",
      "textHide2-5instant",
      "textHide2-6instant",
      "textHide2-7instant",
      // "textHide3-1instant",
      // "textHide3-2instant",
      // "textHide3-3instant",
      // "textHide4-1instant",
      // "textHide4-2instant",
      // "textHide4-3instant",
      // "textHide4-4instant",
      // "textHide4-5instant",
      // "textHide4-6instant",
      "fadeOutRules",
      // "fadeOutPuzzle",
      // "fadeOutD&I",
      "fadeOutWelcome",
    };
    startAnimationGroup(animations);
  }
  // ^^

  void onAnimationFinished(UIAnimationEndEvent e)
  {
    Debug.Log("Anim finished: " + e.name);
    foreach (var animationGroup in animationGroups)
    {
      if (animationGroup.Contains(e.name))
      {
        animationGroup.RemoveAll(name => name == e.name);
        if (animationGroup.Count > 0)
        {
          startAnimation(animationGroup[0]);
        }
      }
    }
    // Remove completed animation groups
    animationGroups.RemoveAll(group => group.Count == 0);
  }
}

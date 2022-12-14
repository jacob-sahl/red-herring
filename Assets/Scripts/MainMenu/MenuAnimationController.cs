using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuAnimationController : MonoBehaviour
{
  public VoiceOver _VO;
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
      "fadeMainMenuContentOut", "backdropIn", "fadeIntro1In", "fadeInWelcome", "textReveal1-1"
      });
    _VO.playClip(0, delay: 0.5f, volume: 1f);
  }

  public void returnToTitleScreen()
  {
    resetAllIntroContent();
    startAnimationGroup(new List<string> { "fadeIntro1Out", "backdropOut", "fadeMainMenuContentIn" });
  }

  public void resetAllIntroContent()
  {
    List<string> animations = new List<string> {
      "textHide1-1instant",
      "textHide1-2instant",
      "textHide1-3instant",
      "textHide1-4instant",
      "textHide1-5instant",
      "textHide1-6instant",
      "textHide1-7instant",
      "textHide1-8instant",
      "textHide1-9instant",
      "fadeOutWelcome",
      "crownOut",
      "playerTokensOut",
      "detectiveGraphicOut",
      "cardGraphicOut",
      "informantGraphicOut",
      "P1reset",
      "P2reset",
      "P3reset",
      "P4reset",
    };
    foreach (string animation in animations)
    {
      startAnimation(animation);
    }
    // startAnimationGroup(animations);
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

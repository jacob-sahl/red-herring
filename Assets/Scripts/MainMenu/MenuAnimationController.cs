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
    startAnimationGroup(new List<string> { "fadeMainMenuContentOut", "expandBackdrop", "fadeIntro1In", "textReveal1-1" });
  }

  public void returnToTitleScreen()
  {
    startAnimationGroup(new List<string> { "textHide1-1", "fadeIntro1Out", "collapseBackdrop", "fadeMainMenuContentIn" });
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
        startAnimation(animationGroup[0]);
      }
    }
    // Remove completed animation groups
    foreach (var animationGroup in animationGroups)
    {
      if (animationGroup.Count == 0)
      {
        animationGroups.Remove(animationGroup);
      }
    }
  }
}

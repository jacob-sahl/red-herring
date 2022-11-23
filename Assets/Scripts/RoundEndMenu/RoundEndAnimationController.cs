using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundEndAnimationController : MonoBehaviour
{
  private List<List<string>> animationGroups;
  void Awake()
  {
    EventManager.AddListener<UIAnimationEndEvent>(onAnimationFinished);
    animationGroups = new List<List<string>>();
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

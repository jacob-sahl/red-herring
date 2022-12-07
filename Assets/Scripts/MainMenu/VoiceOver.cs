using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceOver : MonoBehaviour
{
  public List<AudioClip> voiceOverClips;
  AudioSource source;
  private void Start()
  {
    source = GetComponent<AudioSource>();
  }

  public void playClip(int index, float delay = 0f, float volume = 1f)
  {
    source.volume = volume;
    source.clip = voiceOverClips[index];
    source.PlayDelayed(delay);
  }
}

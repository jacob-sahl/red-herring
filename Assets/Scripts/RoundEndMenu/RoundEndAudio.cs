using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundEndAudio : MonoBehaviour
{
  public AudioSource source;
  public AudioClip successMusic;
  public AudioClip failureMusic;
  public void playSuccessMusic()
  {
    source.clip = successMusic;
    source.Play();
  }
  public void playFailureMusic()
  {
    source.clip = failureMusic;
    source.Play();
  }
}

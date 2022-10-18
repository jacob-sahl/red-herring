using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
  public AudioClip mistakeAudio;
  public AudioClip successAudio;
  public AudioClip backgroundLoop;
  public AudioClip gameLoop;
  private AudioSource source;

  void Start()
  {
    source = GetComponent<AudioSource>();
    source.volume = 0.5f;
    source.clip = backgroundLoop;
    source.Play();
  }

  public void timedGameStart()
  {
    source.Stop();
    source.clip = gameLoop;
    source.Play();
  }

  public void playMistake()
  {
    source.PlayOneShot(mistakeAudio);
  }
  public void playSuccess()
  {
    source.Stop();
    source.volume = 1.0f;
    source.PlayOneShot(successAudio, 1f);
  }
}

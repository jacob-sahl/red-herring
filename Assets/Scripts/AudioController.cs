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
        source.clip = backgroundLoop;
        source.Play();
    }

    public void timedGameStart() {
        source.Stop();
        source.clip = gameLoop;
        source.Play();
    }

    public void playMistake() {
        source.PlayOneShot(mistakeAudio);
    }
    public void playSuccess() {
        source.PlayOneShot(successAudio);
    }
}

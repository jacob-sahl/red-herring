using UnityEngine;

public class AudioController : MonoBehaviour
{
  public AudioClip mistakeAudio;
  public AudioClip successAudio;
  private AudioSource source;

  private void Start()
  {
    source = GetComponent<AudioSource>();
  }

  public void playMistake()
  {
    source.PlayOneShot(mistakeAudio, 0.6f);
  }

  public void playSuccess()
  {
    // source.Stop();
    // source.volume = 1.0f;
    // source.PlayOneShot(successAudio, 1f);
  }
}
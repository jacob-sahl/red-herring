using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PauseScreen : MonoBehaviour
{
  [Header("Parameters")]
  private bool pauseRelased = false;
  private PlayerInputHandler _inputHandler;
  private GameObject PauseText; //= GameObject.Find("PauseText");
                                // Start is called before the first frame update
  private Detective detective;
  void Start()
  {
    pauseRelased = false;
    //_inputHandler = GetComponent<PlayerInputHandler>();
    PauseText = GameObject.Find("PauseText");
    PauseText.GetComponent<UnityEngine.UI.Text>().text = "";
    detective = GameObject.Find("Detective").GetComponent<Detective>();
  }

  // Update is called once per frame
  void Update()
  {
    if (_inputHandler.GetPause() == true)
    {
      Pause();
    }
    else
    {
      pauseRelased = true;
    }
  }

  public bool gamePaused()
  {
    return !pauseRelased;
  }

  void Pause()
  {

    if (Time.timeScale == 0.0f && pauseRelased)
    {
      Time.timeScale = 1f;
      detective.frozen = false;
      pauseRelased = false;
      Debug.Log("unpaused");
      PauseText.GetComponent<UnityEngine.UI.Text>().text = "";
      //PauseText.SetActive(false); //.getComponent<MeshRenderer>().enabled = false;
    }
    if (Time.timeScale == 1.0f && pauseRelased)
    {
      Time.timeScale = 0f;
      detective.frozen = true;
      pauseRelased = false;
      Debug.Log("paused");
      PauseText.GetComponent<UnityEngine.UI.Text>().text = "Game Paused";
      //PauseText.SetActive(true); //.getComponent<MeshRenderer>().enabled = true;
    }
  }

  public void assignInputHandler(PlayerInputHandler handler)
  {
    _inputHandler = handler;
  }
}

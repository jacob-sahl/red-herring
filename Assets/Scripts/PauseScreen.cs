using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using TMPro;

public class PauseScreen : MonoBehaviour
{
  [Header("Parameters")]
  private bool pauseRelased = false;
  private PlayerInputHandler _inputHandler;
  private Detective detective;
  void Start()
  {
    pauseRelased = false;
    //_inputHandler = GetComponent<PlayerInputHandler>();
    detective = GameObject.Find("Detective").GetComponent<Detective>();
    setPauseScreenChildrenActive(false);
  }

  void setPauseScreenChildrenActive(bool state)
  {
    for (int i = 0; i < transform.childCount; i++)
    {
      transform.GetChild(i).gameObject.SetActive(state);
    }
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
      setPauseScreenChildrenActive(false);
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;
    }
    if (Time.timeScale == 1.0f && pauseRelased)
    {
      Time.timeScale = 0f;
      detective.frozen = true;
      pauseRelased = false;
      Debug.Log("paused");
      setPauseScreenChildrenActive(true);
      Cursor.visible = true;
      Cursor.lockState = CursorLockMode.None;
    }
  }

  public void assignInputHandler(PlayerInputHandler handler)
  {
    _inputHandler = handler;
  }
}

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
  private bool pauseReleased = false;
  private PlayerInputHandler _inputHandler;
  private Detective detective;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _optionsMenu;
    void Start()
  {
    pauseReleased = false;
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
      pauseReleased = true;
    }
  }

  public bool gamePaused()
  {
    return !pauseReleased;
  }

  void Pause()
  {

    if (Time.timeScale == 0.0f && pauseReleased)
    {
      Time.timeScale = 1f;
      detective.frozen = false;
      pauseReleased = false;
      Debug.Log("unpaused");
      setPauseScreenChildrenActive(false);
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;
    }
    if (Time.timeScale == 1.0f && pauseReleased)
    {
      Time.timeScale = 0f;
      detective.frozen = true;
      pauseReleased = false;
      Debug.Log("paused");
      setPauseScreenChildrenActive(true);
      Cursor.visible = true;
      Cursor.lockState = CursorLockMode.None;
    }
  }

    public void ResumeButton()
    {
        Time.timeScale = 1f;
        pauseReleased = false;
        _pauseMenu.SetActive(false);
        _optionsMenu.SetActive(false);
        Debug.Log("unpaused");
    }

  public void assignInputHandler(PlayerInputHandler handler)
  {
    _inputHandler = handler;
  }
}

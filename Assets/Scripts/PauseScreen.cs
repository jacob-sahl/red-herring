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
    private GameController gamecontroller;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _optionsMenu;
    //private LevelManager _levelManager;

    void Start()
  {
    pauseReleased = false;
    //_inputHandler = GetComponent<PlayerInputHandler>();
    detective = GameObject.Find("Detective").GetComponent<Detective>();
    //levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        setPauseScreenChildrenActive(false);
    gamecontroller = GameController.Instance;
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
      _optionsMenu.SetActive(false);
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
        pauseReleased = true;
        Pause();
        //_pauseMenu.SetActive(false);
        _optionsMenu.SetActive(false);
        Debug.Log("unpaused");
    }

    /*public void QuitButton()
    {
        levelManager.EndLevel();
    }*/

    public void ChangeMouseSensitivity(float value)
    {
        gamecontroller.updateMouseSensitivity(value);
    }

    public void ChangeRotationSpeed(float value)
    {
        gamecontroller.updateObjectRotationSpeed(value);
    }

    public void ChangeRoundDuration(string value)
    {
        gamecontroller.updateMinutesPerRound(value);
    }

    public void assignInputHandler(PlayerInputHandler handler)
  {
    _inputHandler = handler;
  }
}

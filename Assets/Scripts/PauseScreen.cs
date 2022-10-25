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
    void Start()
    {
        pauseRelased = false;
        //_inputHandler = GetComponent<PlayerInputHandler>();
        PauseText = GameObject.Find("PauseText");
        PauseText.GetComponent<UnityEngine.UI.Text>().text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputHandler.GetPause() == true)
        {
            Pause();
        } else
        {
            pauseRelased = true;
        }
    }
    void Pause ()
    {
        
        if (Time.timeScale == 0.0f && pauseRelased)
        {
            Time.timeScale = 1f;

            pauseRelased = false;
            Debug.Log("unpaused");
            PauseText.GetComponent<UnityEngine.UI.Text>().text = "";
            //PauseText.SetActive(false); //.getComponent<MeshRenderer>().enabled = false;
        }
        if (Time.timeScale == 1.0f && pauseRelased)
        {
            Time.timeScale = 0f;

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

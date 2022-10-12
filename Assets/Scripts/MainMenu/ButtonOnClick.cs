using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonOnClick : MonoBehaviour
{
    public void ExitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    
    public void StartGame()
    {
        Debug.Log("Start");
        GameController.Instance.LoadLevel();
    }
    
    public void LoadMenu()
    {
        Debug.Log("Menu");
        GameController.Instance.LoadMenuScene();
    }
    
    public void SetupLevel()
    {
        Debug.Log("Setup");
        GameController.Instance.SetupLevel();
    }
}

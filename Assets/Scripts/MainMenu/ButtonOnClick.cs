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
        GameController.Instance.LoadPuzzle();
    }
    
    public void LoadMenu()
    {
        Debug.Log("Menu");
        GameController.Instance.LoadMenuScene();
    }
}

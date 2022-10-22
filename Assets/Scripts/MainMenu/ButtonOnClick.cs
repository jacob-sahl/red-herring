using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        UndoDontDestroyOnLoad();
        SceneManager.LoadScene("Menu");
    }
    
    public void SetupLevel()
    {
        Debug.Log("Setup");
        GameController.Instance.SetupLevel();
    }
    
    private void UndoDontDestroyOnLoad()
    {
        foreach (GameObject go in GetDontDestroyOnLoadObjects())
        {
            SceneManager.MoveGameObjectToScene(go, SceneManager.GetActiveScene());
        }
    }
    
    public static GameObject[] GetDontDestroyOnLoadObjects()
    {
        GameObject temp = null;
        try
        {
            temp = new GameObject();
            Object.DontDestroyOnLoad( temp );
            UnityEngine.SceneManagement.Scene dontDestroyOnLoad = temp.scene;
            Object.DestroyImmediate( temp );
            temp = null;
     
            return dontDestroyOnLoad.GetRootGameObjects();
        }
        finally
        {
            if( temp != null )
                Object.DestroyImmediate( temp );
        }
    }

}

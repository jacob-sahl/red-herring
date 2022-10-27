using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class ButtonOnClick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
  public TextMeshProUGUI theText;
  void Start()
  {
    theText = GetComponentInChildren<TextMeshProUGUI>();
  }

  public void OnPointerEnter(PointerEventData eventData) // mouse hovers over button
  {
    //Debug.Log(theText.text+ " bolded");
    theText.fontStyle = FontStyles.Bold;
  }

  public void OnPointerExit(PointerEventData eventData) // mouse hovers off button
  {
    theText.fontStyle = FontStyles.Normal;
  }

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
      Object.DontDestroyOnLoad(temp);
      UnityEngine.SceneManagement.Scene dontDestroyOnLoad = temp.scene;
      Object.DestroyImmediate(temp);
      temp = null;

      return dontDestroyOnLoad.GetRootGameObjects();
    }
    finally
    {
      if (temp != null)
        Object.DestroyImmediate(temp);
    }
  }

}

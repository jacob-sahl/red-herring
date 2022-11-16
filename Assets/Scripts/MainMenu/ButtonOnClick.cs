using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Utils;

public class ButtonOnClick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI theText;

    private void Start()
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
        foreach (var go in GetDontDestroyOnLoadObjects())
            SceneManager.MoveGameObjectToScene(go, SceneManager.GetActiveScene());
    }

    public static GameObject[] GetDontDestroyOnLoadObjects()
    {
        GameObject temp = null;
        try
        {
            temp = new GameObject();
            DontDestroyOnLoad(temp);
            var dontDestroyOnLoad = temp.scene;
            DestroyImmediate(temp);
            temp = null;

            return dontDestroyOnLoad.GetRootGameObjects();
        }
        finally
        {
            if (temp != null)
                DestroyImmediate(temp);
        }
    }

    public void CreateGame()
    {
        APIClient.APIClient.Instance.CreateGameInstance().Then(instance =>
        {
            Events.GameCreatedEvent.gameInstance = instance;
            EventManager.Broadcast(Events.GameCreatedEvent);
            Dispatcher.Instance.RunInMainThread(() => { GameController.Instance.gameInstance = instance; GameController.Instance.PlayerManager.ClearPlayers(); });
        });
        Debug.Log("Create");
    }
}
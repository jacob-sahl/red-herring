using APIClient;
using TMPro;
using UnityEngine;

namespace MainMenu
{
    public class JoinGameMenu : MonoBehaviour
    {
        private bool ready;

        private void Awake()
        {
            if (GameController.Instance.gameInstance != null)
            {
                PopulateGameInfo(GameController.Instance.gameInstance);
            }
            EventManager.AddListener<GameCreatedEvent>(onGameCreated);
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener<GameCreatedEvent>(onGameCreated);
        }

        private void onGameCreated(GameCreatedEvent e)
        {
            PopulateGameInfo(e.gameInstance);
        }

        private void PopulateGameInfo(GameInstance gameInstance)
        {
            transform.Find("GameID").GetComponent<TMP_Text>().text = gameInstance.id;
            transform.Find("JoinCode").GetComponent<TMP_Text>().text = gameInstance.joinCode;
            ready = true;
        }
    }
}
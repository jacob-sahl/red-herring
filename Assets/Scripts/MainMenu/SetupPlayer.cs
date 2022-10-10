using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupPlayer : MonoBehaviour
{
    public List<GameObject> players = new List<GameObject>();
    private void Awake()
    {
        EventManager.AddListener<PlayerJoinedEvent>(onPlayerJoined);
        EventManager.AddListener<PlayerUpdateEvent>(onPlayerUpdated);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void onPlayerJoined(PlayerJoinedEvent e)
    {
        Debug.Log("SetupPlayer: Player Joined");
        JoinPlayer(e.PlayerID);
    }
    
    private void JoinPlayer(int playerID)
    {
        GameObject player = players[playerID];
        player.transform.Find("JoinPrompt").gameObject.SetActive(false);
        var avatar = player.transform.Find("Avatar");
        avatar.GetComponent<Image>().color = PlayerManager.Instance.getPlayer(playerID).color;
        avatar.gameObject.SetActive(true);
    }
    
    private void onPlayerUpdated(PlayerUpdateEvent e)
    {
        Debug.Log("SetupPlayer: Player Updated");
        int playerID = e.PlayerID;
        GameObject player = players[playerID];
        player.transform.Find("JoinPrompt").gameObject.SetActive(false);
        var avatar = player.transform.Find("Avatar");
        avatar.GetComponent<Image>().color = PlayerManager.Instance.getPlayer(playerID).color;
        avatar.gameObject.SetActive(true);
    }
}

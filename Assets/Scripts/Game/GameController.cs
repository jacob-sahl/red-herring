using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public PlayerManager PlayerManager;
    void Awake() {
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerManager = PlayerManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    public void LoadScene(string name) {
        SceneManager.LoadScene(name);
    }
    
    public void LoadMenuScene() {
        LoadScene("Menu");
    }
    
    public void LoadEndScene() {
        LoadScene("End");
    }
    
    public void LoadPuzzle() {
        LoadScene("_MAINSCENE");
    }
}
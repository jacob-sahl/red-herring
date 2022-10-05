using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
  public static PlayerManager Instance { get; private set; }
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
  
  private int nPlayers;

  void Start()
  {
    nPlayers = 0;
  }
  public int addPlayer()
  {
    nPlayers += 1;
    return (nPlayers - 1);
  }

}

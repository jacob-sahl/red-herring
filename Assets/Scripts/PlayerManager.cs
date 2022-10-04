using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
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

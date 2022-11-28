using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundEndButtons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameController.Instance._gameEnd)
        {
            transform.Find("RoundEndButtons").gameObject.SetActive(false);
            transform.Find("GameEndButtons").gameObject.SetActive(true);
        }
    }
}

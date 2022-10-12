using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Puzzle : MonoBehaviour
{
    public string puzzleName;
    public LevelManager levelManager;
    public delegate void completeCallback();
    private completeCallback _complete_callback;
    public bool isComplete = false;

    public virtual void Awake()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        levelManager.addPuzzle(this);
    }

    public void SetCompleteCallback(completeCallback callback)
    {
        _complete_callback = callback;
    }
    
    public void Complete()
    {
        isComplete = true;
        if (_complete_callback != null)
        {
            _complete_callback();
        }
    }
}

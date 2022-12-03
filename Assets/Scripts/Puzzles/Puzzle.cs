using UnityEngine;

public abstract class Puzzle : MonoBehaviour
{
  public delegate void completeCallback();

  public string puzzleName;
  public LevelManager levelManager;
  public bool isComplete;
  private completeCallback _complete_callback;

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
    if (_complete_callback != null) _complete_callback();
  }
}
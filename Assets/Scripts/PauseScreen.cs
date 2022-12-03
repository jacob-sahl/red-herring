using UnityEngine;

public class PauseScreen : MonoBehaviour
{
  private PlayerInputHandler _inputHandler;
  private Detective detective;
  [Header("Parameters")] private bool pauseReleased;
  private GameController gamecontroller;
  [SerializeField] private GameObject _pauseMenu;
  [SerializeField] private GameObject _optionsMenu;
  [SerializeField] private GameObject _controlPrompts;

  private void Start()
  {
    pauseReleased = false;
    //_inputHandler = GetComponent<PlayerInputHandler>();
    detective = GameObject.Find("Detective").GetComponent<Detective>();
    setPauseScreenChildrenActive(false);
    gamecontroller = GameController.Instance;
    detective = GameObject.Find("Detective").GetComponent<Detective>();
  }

  // Update is called once per frame
  private void Update()
  {
    if (_inputHandler.GetPause())
      Pause();
    else
      pauseReleased = true;
  }

  private void setPauseScreenChildrenActive(bool state)
  {
    for (var i = 0; i < transform.childCount; i++) transform.GetChild(i).gameObject.SetActive(state);
  }

  public bool gamePaused()
  {
    return !pauseReleased;
  }

  private void Pause()
  {
    if (Time.timeScale == 0.0f && pauseReleased) // unpause
    {
      Time.timeScale = 1f;
      detective.frozen = false;
      pauseReleased = false;
      // Debug.Log("unpaused");
      setPauseScreenChildrenActive(false);
      _optionsMenu.SetActive(false);
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;
    }

    if (Time.timeScale == 1.0f && pauseReleased) // pause
    {
      Time.timeScale = 0f;
      detective.frozen = true;
      pauseReleased = false;
      // Debug.Log("paused");
      setPauseScreenChildrenActive(true);
      _optionsMenu.SetActive(false);
      Cursor.visible = true;
      Cursor.lockState = CursorLockMode.None;
    }
  }

  public void ResumeButton()
  {
    pauseReleased = true;
    Pause();
    //_pauseMenu.SetActive(false);
    _optionsMenu.SetActive(false);
    // Debug.Log("unpaused");
  }

  public void ChangeMouseSensitivity(float value)
  {
    gamecontroller.updateMouseSensitivity(value);
  }

  public void ChangeRotationSpeed(float value)
  {
    gamecontroller.updateObjectRotationSpeed(value);
  }

  public void ChangeRoundDuration(string value)
  {
    gamecontroller.updateMinutesPerRound(value);
  }

  public void assignInputHandler(PlayerInputHandler handler)
  {
    _inputHandler = handler;
  }
}
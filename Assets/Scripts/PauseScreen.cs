using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    private PlayerInputHandler _inputHandler;
    private Detective detective;

    [Header("Parameters")] private bool pauseRelased;

    private void Start()
    {
        pauseRelased = false;
        //_inputHandler = GetComponent<PlayerInputHandler>();
        detective = GameObject.Find("Detective").GetComponent<Detective>();
        setPauseScreenChildrenActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (_inputHandler.GetPause())
            Pause();
        else
            pauseRelased = true;
    }

    private void setPauseScreenChildrenActive(bool state)
    {
        for (var i = 0; i < transform.childCount; i++) transform.GetChild(i).gameObject.SetActive(state);
    }

    public bool gamePaused()
    {
        return !pauseRelased;
    }

    private void Pause()
    {
        if (Time.timeScale == 0.0f && pauseRelased)
        {
            Time.timeScale = 1f;
            detective.frozen = false;
            pauseRelased = false;
            Debug.Log("unpaused");
            setPauseScreenChildrenActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Time.timeScale == 1.0f && pauseRelased)
        {
            Time.timeScale = 0f;
            detective.frozen = true;
            pauseRelased = false;
            Debug.Log("paused");
            setPauseScreenChildrenActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void assignInputHandler(PlayerInputHandler handler)
    {
        _inputHandler = handler;
    }
}
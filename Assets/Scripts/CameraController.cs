using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    private Camera playerCam;

    private void Start()
    {
        playerCam = GetComponent<Camera>();
        EventManager.AddListener<FocusEvent>(OnFocus);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<FocusEvent>(OnFocus);
    }

    public void OnFocus(FocusEvent evt)
    {
        var target = evt.gameObject;
        Focus(target);
    }

    private void Focus(GameObject target)
    {
        playerCam.transform.LookAt(target.transform);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICursorRaycast : MonoBehaviour
{
  [Header("References")]
  [Tooltip("Reference to the main camera used for the player")]
  public Camera playerCamera;
  ICursorController _controller;
  Outline _lastOutline;
  void Start()
  {
    _controller = GetComponent<ICursorController>();
    playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
  }

  void Update()
  {
    // if (!_controller._dev_handling) return;
    RaycastHit hit;
    Vector3 position = _controller.GetPosition();
    Ray ray = playerCamera.ScreenPointToRay(position);
    if (Physics.Raycast(ray, out hit))
    {
      //   ICursorHoverEvent evt = Events.ICursorHoverEvent;
      //   evt.ObjectTag = hit.collider.gameObject.tag;
      //   EventManager.Broadcast(evt);
      Color iColor = _controller.color;
      // Debug.DrawRay(ray.origin, ray.direction, iColor, 10);
      var colliderGameObject = hit.collider.gameObject;
      var outline = colliderGameObject.GetComponent<Outline>();
      if (outline != null)
      {
        if (_lastOutline != null && _lastOutline != outline)
        {
          _lastOutline.enabled = false;
        }
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineWidth = 5;
        outline.OutlineColor = iColor;
        outline.enabled = true;
        _lastOutline = outline;
      }
      else
      {
        if (_lastOutline != null)
        {
          _lastOutline.enabled = false;
        }
      }
    }
  }
}

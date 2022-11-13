using UnityEngine;

public class Skull : MonoBehaviour
{
    private bool broadcastedOffShelf;
    private Vector3 initPos;

    private void Start()
    {
        initPos = transform.position;
    }

    private void Update()
    {
        if (initPos.y - transform.position.y > 1 && !broadcastedOffShelf)
        {
            var e = new SecretObjectiveEvent();
            e.id = SecretObjectiveID.SkullOffShelf;
            e.status = true;
            EventManager.Broadcast(e);
            broadcastedOffShelf = true;
        }
    }
}
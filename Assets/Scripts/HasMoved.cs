using UnityEngine;

public class HasMoved : MonoBehaviour
{
    public float sensitivity = 0.1f;
    private Vector3 initPos;
    private bool moved;

    private void Start()
    {
        moved = false;
        initPos = transform.position;
    }

    private void Update()
    {
        if (!moved)
            if (
                    Mathf.Abs(transform.position.x - initPos.x) > sensitivity ||
                    Mathf.Abs(transform.position.y - initPos.y) > sensitivity ||
                    Mathf.Abs(transform.position.z - initPos.z) > sensitivity
                )
                // Debug.Log("Moved: " + gameObject.name);
                moved = true;
    }

    public bool hasMoved()
    {
        return moved;
    }
}
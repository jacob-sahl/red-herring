using UnityEngine;

public class LeftShelf : MonoBehaviour
{
    private Books[] books;

    private void Start()
    {
        books = gameObject.GetComponentsInChildren<Books>();
        setUpBooks();
    }

    private void setUpBooks()
    {
        var puzzle = GameController.Instance.getCurrentPuzzle().id;
        switch (puzzle)
        {
            case TypeWriterPuzzleID.BlueRedYellow:
                showObject(books[0].gameObject);
                books[0].gameObject.GetComponent<MeshRenderer>().materials[1].color =
                    new Color(0f, 0f, 1f, 0.05f); // Blue
                books[1].gameObject.GetComponent<MeshRenderer>().materials[1].color =
                    new Color(1f, 0f, 0f, 0.05f); // Red
                books[2].gameObject.GetComponent<MeshRenderer>().materials[1].color =
                    new Color(1f, 1f, 0f, 0.05f); // Yellow
                break;
        }
    }

    private void showObject(GameObject obj)
    {
        var renderer = obj.GetComponent<Renderer>();
        if (renderer != null) renderer.enabled = true;
        var collider = obj.GetComponent<Collider>();
        if (collider != null) collider.enabled = true;
    }

    private void hideObject(GameObject obj)
    {
        var renderer = obj.GetComponent<Renderer>();
        if (renderer != null) renderer.enabled = false;
        var collider = obj.GetComponent<Collider>();
        if (collider != null) collider.enabled = false;
    }
}
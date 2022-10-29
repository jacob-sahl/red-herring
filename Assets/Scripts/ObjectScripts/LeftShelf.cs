using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftShelf : MonoBehaviour
{
  private Books[] books;
  void Start()
  {
    books = gameObject.GetComponentsInChildren<Books>();
    setUpBooks();
  }

  void setUpBooks()
  {
    TypeWriterPuzzleID puzzle = GameController.Instance.getCurrentPuzzle().id;
    switch (puzzle)
    {
      case TypeWriterPuzzleID.BlueRedYellow:
        books[0].gameObject.GetComponent<MeshRenderer>().materials[1].color = new Color(0f, 0f, 1f, 0.05f); // Blue
        books[1].gameObject.GetComponent<MeshRenderer>().materials[1].color = new Color(1f, 0f, 0f, 0.05f); // Red
        books[2].gameObject.GetComponent<MeshRenderer>().materials[1].color = new Color(1f, 1f, 0f, 0.05f); // Yellow
        break;
      default:
        books[0].gameObject.SetActive(false);
        break;
    }
  }

  // Update is called once per frame
  void Update()
  {

  }
}

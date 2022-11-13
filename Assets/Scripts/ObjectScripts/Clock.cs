using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public float rotationVerticalOffset = 12.765f;
    public GameObject debugSpherePrefab;
    public int clockTimeMinutes;
    private Vector3 axis;
    private readonly List<SecretObjectiveID> broadcasted = new();
    private GameObject clockHead;
    private GameObject handCentre;
    private GameObject hourHand;
    private GameObject minuteHand;
    private Vector3 pivot;
    private TypeWriterPuzzleInstance puzzle;

    private void Awake()
    {
        EventManager.AddListener<InteractEvent>(onInteract);
    }

    private void Start()
    {
        clockTimeMinutes = 180;
        puzzle = GameController.Instance.getCurrentPuzzle();
        handCentre = transform.Find("head/handCentre").gameObject;
        minuteHand = transform.Find("head/minuteHand").gameObject;
        hourHand = transform.Find("head/hourHand").gameObject;
        clockHead = transform.Find("head").gameObject;
        setUpRotation();
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener<InteractEvent>(onInteract);
    }

    private void setUpRotation()
    {
        pivot = handCentre.transform.position;
        pivot += new Vector3(0, rotationVerticalOffset, 0);
        // GameObject sphere = Instantiate(debugSpherePrefab);
        // sphere.transform.position = pivot;
        // sphere.name = "DEBUG SPHERE";
        axis = handCentre.transform.up;
    }

    private void onInteract(InteractEvent e)
    {
        if (e.gameObject == clockHead) rotateHands();
    }

    private void rotateHands()
    {
        minuteHand.transform.RotateAround(pivot, axis, -30f);
        hourHand.transform.RotateAround(pivot, axis, -2.5f);
        clockTimeMinutes += 5;

        if (clockTimeMinutes >= 345 && !broadcasted.Contains(SecretObjectiveID.SetClockTo545))
        {
            var evt = new SecretObjectiveEvent();
            evt.id = SecretObjectiveID.SetClockTo545;
            evt.status = true;
            EventManager.Broadcast(evt);
            broadcasted.Add(SecretObjectiveID.SetClockTo545);
        }

        if (clockTimeMinutes >= 720) clockTimeMinutes = 0;
        var e = new ClockTimeChangeEvent();
        e.minutes = clockTimeMinutes;
        EventManager.Broadcast(e);

        // Reveal clues for Puzzle #2
        if (puzzle.id == TypeWriterPuzzleID.One2Three)
        {
            if (clockTimeMinutes >= 6 * 60)
            {
                var text = transform.Find("head/clockCanvas/III").gameObject;
                if (text != null) text.GetComponent<Renderer>().enabled = true;
            }
            else if (clockTimeMinutes >= 5 * 60)
            {
                var text = transform.Find("head/clockCanvas/II").gameObject;
                if (text != null) text.GetComponent<Renderer>().enabled = true;
            }
            else if (clockTimeMinutes >= 4 * 60)
            {
                var text = transform.Find("head/clockCanvas/I").gameObject;
                if (text != null) text.GetComponent<Renderer>().enabled = true;
            }
        }
    }
}
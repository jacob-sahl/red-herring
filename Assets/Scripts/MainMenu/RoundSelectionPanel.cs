using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundSelectionPanel : MonoBehaviour
{
    
    public GameObject ToggleButton;
    public GameObject RoundDropdown;
    
    private bool _expanded;
    private RectTransform _reactTransform;
    
    public bool expanded
    {
        get { return _expanded; }
        set
        {
            _expanded = value;
            ToggleButton.GetComponent<Transform>().transform.Find("ButtonText").GetComponent<TMP_Text>().text = _expanded ? "<" : ">";
            _reactTransform.position = _expanded ? new Vector3(0, _reactTransform.position.y, _reactTransform.position.z) : new Vector3(-260, _reactTransform.position.y, _reactTransform.position.z);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _reactTransform = GetComponent<RectTransform>();
        expanded = false;
        ToggleButton.GetComponent<Button>().onClick.AddListener(ToggleExpanded);
        RoundDropdown.GetComponent<TMP_Dropdown>().onValueChanged.AddListener(OnRoundSelected);
    }
    
    public void ToggleExpanded()
    {
        expanded = !expanded;
    }
    
    public void OnRoundSelected(int round)
    {
        Debug.Log("Round selected: " + round);
        GameController.Instance.SetRound(round);
    }
}

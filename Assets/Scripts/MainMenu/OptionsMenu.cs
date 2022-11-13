using Game;
using TMPro;
using UnityEngine;

namespace MainMenu
{
    public class OptionsMenu : MonoBehaviour
    {
        public bool changed;
        private GameObject RoundTime;

        private void Start()
        {
            EventManager.AddListener<GamePreferenceChangeEvent>(OnPreferenceChange);
            RoundTime = GameObject.Find("RoundTime");
            UpdateField();
        }

        private void Update()
        {
            if (changed)
            {
                changed = false;
                UpdateField();
            }
        }

        private void OnDestroy()
        {
            EventManager.RemoveListener<GamePreferenceChangeEvent>(OnPreferenceChange);
        }

        private void OnPreferenceChange(GamePreferenceChangeEvent e)
        {
            // Do something with the event
            Debug.Log("Preference changed");
            changed = true;
        }

        private void UpdateField()
        {
            RoundTime.GetComponent<TMP_InputField>().text = GamePreferences.MinutesPerRound.ToString();
        }
    }
}
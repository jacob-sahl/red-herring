using System;
using Game;
using TMPro;
using UnityEngine;

namespace MainMenu
{
    public class OptionsMenu : MonoBehaviour
    {
        public bool changed = false;
        private GameObject RoundTime;
        void Start()
        {
            EventManager.AddListener<GamePreferenceChangeEvent>(OnPreferenceChange);
            RoundTime = GameObject.Find("RoundTime");
            UpdateField();
        }
        
        void OnDestroy()
        {
            EventManager.RemoveListener<GamePreferenceChangeEvent>(OnPreferenceChange);
        }

        private void Update()
        {
            if (changed)
            {
                changed = false;
                UpdateField();
            }
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
using UnityEngine;

namespace Game
{
    public class GamePreferences
    {
        public static int MinutesPerRound;

        public static void Save()
        {
            PlayerPrefs.SetInt("MinutesPerRound", MinutesPerRound);
            PlayerPrefs.Save();
            EventManager.Broadcast(Events.GamePreferenceChangeEvent);
        }

        public static void Load()
        {
            MinutesPerRound = PlayerPrefs.GetInt("MinutesPerRound", 5);
        }
    }
}
using System;
using UnityEngine;

namespace Project.Runtime.Scripts.Core
{
    public static class PlayerPrefsManager
    {
        public static string PlayerId
        {
            get => PlayerPrefs.GetString("PlayerId");
            set => PlayerPrefs.SetString("PlayerId", value);
        }

        public static event EventHandler IsSfxActivatedUpdatedEvent;
        public static bool IsSfxActivated
        {
            get
            {
                if (PlayerPrefs.HasKey("isSfxActivated")) 
                    return PlayerPrefs.GetInt("isSfxActivated") != 0;
                
                PlayerPrefs.SetInt("isSfxActivated", 1);
                PlayerPrefs.Save();
                return PlayerPrefs.GetInt("isSfxActivated") != 0;
            }
            set
            {
                PlayerPrefs.SetInt("isSfxActivated", !value ? 0 : 1);
                IsSfxActivatedUpdatedEvent?.Invoke(null, EventArgs.Empty);
            }
        }

        public static event EventHandler IsMusicActivatedUpdatedEvent;
        public static bool IsMusicActivated
        {
            get
            {
                if (PlayerPrefs.HasKey("isMusicActivated")) 
                    return PlayerPrefs.GetInt("isMusicActivated") != 0;
                
                PlayerPrefs.SetInt("isMusicActivated", 1);
                PlayerPrefs.Save();
                return PlayerPrefs.GetInt("isMusicActivated") != 0;
            }
            set
            {
                PlayerPrefs.SetInt("isMusicActivated", !value ? 0 : 1);
                IsMusicActivatedUpdatedEvent?.Invoke(null, EventArgs.Empty);
            }
        }
        
        public static int EnemiesKilled
        {
            get => PlayerPrefs.GetInt("EnemiesKilled", 0); // Default to 0 if not set
            set
            {
                PlayerPrefs.SetInt("EnemiesKilled", value);
                PlayerPrefs.Save();
            }
        }

        public static string BestTime
        {
            get => PlayerPrefs.GetString("BestTime", "00:00"); // Default to "00:00" if not set
            set
            {
                PlayerPrefs.SetString("BestTime", value);
                PlayerPrefs.Save();
            }
        }
    }
}
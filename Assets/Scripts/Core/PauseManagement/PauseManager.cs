using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.PauseManagement
{
    public class PauseManager
    {
        public static PauseManager Instance
        {
            get
            {
                return _instance ??= new PauseManager();
            }
            private set
            {
                _instance = value;
            }
        }
        private static PauseManager _instance;
        private PauseManager()
        {
            Instance = this;
        }
        public static bool IsPaused => Instance.isPaused;
        private bool isPaused;

        public static void SetPause()
        {
            Instance.isPaused = true;;
        }
        public static void UnsetPause()
        {
            Instance.isPaused = false;
        }
    }
}

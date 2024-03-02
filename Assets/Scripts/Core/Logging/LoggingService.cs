using Core.Resources;
using UnityEngine;

namespace Core.Logging
{
    public class LoggingService : ILoggingService, IGameService
    {
        public void OnServiceRegistering()
        {

        }
        public void Log(string message)
        {
            Debug.Log(message);
        }

        public void LogError(string message)
        {
            Debug.LogError(message);
        }
    }
}
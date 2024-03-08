using Core.Resources;
using UnityEngine;

namespace Core.Logging
{
    internal class LoggingService : ILoggingService, IGameService
    {
        public void OnResourceCreating()
        {
        }
        public void Log(string message, string category = null)
        {
            Debug.Log(message);
        }

        public void LogError(string message)
        {
            Debug.LogError(message);
        }
    }
}
using Core.Resources;
using UnityEngine;

namespace Core.Logging
{
    internal class BuildLoggingService : ILoggingService
    {
        public void OnResourceCreating()
        {
        }
        public void Log(string message, string category = null)
        {
        }

        public void LogError(string message)
        {
        }
    }
}
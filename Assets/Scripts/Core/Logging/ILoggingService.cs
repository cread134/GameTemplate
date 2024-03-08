using Core.Resources;

namespace Core.Logging
{
    public interface ILoggingService : IResourceInstance
    {
        public void Log(string message, string category = null);
        public void LogError(string message);
    }
}
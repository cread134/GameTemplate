using Core.Resources;

namespace Core.Logging
{
    public interface ILoggingService : IResourceInstance
    {
        public void Log(string message);
        public void LogError(string message);
    }
}
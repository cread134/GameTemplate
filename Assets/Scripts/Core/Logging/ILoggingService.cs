namespace Core.Logging
{
    public interface ILoggingService
    {
        public void Log(string message);
        public void LogError(string message);
    }
}
namespace FootballLeague.Core.Contracts.Loger
{
    using System;

    public interface ILoggerService
    {
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message, Exception exception = null);
    }
}

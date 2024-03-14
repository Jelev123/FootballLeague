namespace FootballLeague.Core.Services.Logger
{
    using FootballLeague.Core.Contracts.Loger;
    using System;
    using System.IO;

    public class LoggerService : ILoggerService
    {
        private readonly string _logFilePath;

        public LoggerService(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public void LogError(string message, Exception exception = null)
        {
            LogToFile($"[ERROR] {message}{(exception != null ? ": " + exception.Message : "")}");
        }

        public void LogInfo(string message)
        {
            LogToFile($"[INFO] {message}");
        }

        public void LogWarning(string message)
        {
            LogToFile($"[WARNING] {message}");
        }

        private void LogToFile(string message)
        {
            try
            {
                using (StreamWriter writer = File.AppendText(_logFilePath))
                {
                    writer.WriteLine($"{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")} - {message}");
                }
            }
            catch (Exception ex)
            {
                // If logging to file fails, log the error using Console.WriteLine
                Console.WriteLine($"Error while logging to file: {ex.Message}");
            }
        }
    }
}

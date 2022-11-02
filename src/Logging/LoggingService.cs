using System.Diagnostics;

namespace Logging
{
    public class LoggingService : ILoggingService
    {
        public void Log(string message)
        {
            Debug.WriteLine($"** {message} **");
        }
    }
}

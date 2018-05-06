using System;

namespace Mushka.Accounting.Core.Extensibility.Logging
{
    public interface ILogger
    {
        void LogDebug(string message);

        void LogInfo(string message);

        void LogWarning(string message);

        void LogError(string message);

        void LogError(Exception exception);
    }
}
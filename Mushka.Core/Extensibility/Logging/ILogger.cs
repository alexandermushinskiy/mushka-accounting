using System;

namespace Mushka.Core.Extensibility.Logging
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
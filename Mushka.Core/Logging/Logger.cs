using System;
using Mushka.Core.Extensibility.Logging;
using NLogLogger = NLog.Logger;

namespace Mushka.Core.Logging
{
    internal class Logger : ILogger
    {
        private readonly NLogLogger logger;

        public Logger(NLogLogger logger)
        {
            this.logger = logger;
        }

        public void LogInfo(string message) => logger.Info(message);

        public void LogError(string message) => logger.Error(message);

        public void LogError(Exception exception) => logger.Error(exception);

        public void LogDebug(string message) => logger.Debug(message);

        public void LogWarning(string message) => logger.Warn(message);
    }
}
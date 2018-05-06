using System;
using Mushka.Accounting.Core.Extensibility.Logging;
using NLogLogger = NLog.Logger;

namespace Mushka.Accounting.Core.Logging
{
    internal class Logger : ILogger
    {
        private readonly ITraceIdentifierProvider traceIdentifierProvider;

        private readonly NLogLogger logger;

        public Logger(NLogLogger logger, ITraceIdentifierProvider traceIdentifierProvider)
        {
            this.logger = logger;
            this.traceIdentifierProvider = traceIdentifierProvider;
        }

        public void LogInfo(string message) => logger.Info(message, traceIdentifierProvider.TraceIdentifier);

        public void LogError(string message) => logger.Error(message, traceIdentifierProvider.TraceIdentifier);

        public void LogError(Exception exception) => logger.Error(exception, traceIdentifierProvider.TraceIdentifier);

        public void LogDebug(string message) => logger.Debug(message, traceIdentifierProvider.TraceIdentifier);

        public void LogWarning(string message) => logger.Warn(message, traceIdentifierProvider.TraceIdentifier);
    }
}
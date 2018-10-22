using Mushka.Accounting.Core.Extensibility.Logging;
using NLog;
using ILogger = Mushka.Accounting.Core.Extensibility.Logging.ILogger;

namespace Mushka.Accounting.Core.Logging
{
    internal class LoggerFactory : ILoggerFactory
    {
        private const string DefaultLoggerName = "Mushka.Accounting.";

        public ILogger CreateLogger(string loggerName) => new Logger(LogManager.GetLogger(DefaultLoggerName + loggerName));
    }
}
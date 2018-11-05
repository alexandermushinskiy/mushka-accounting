using Mushka.Core.Extensibility.Logging;
using NLog;
using ILogger = Mushka.Core.Extensibility.Logging.ILogger;

namespace Mushka.Core.Logging
{
    internal class LoggerFactory : ILoggerFactory
    {
        private const string DefaultLoggerName = "Mushka.Accounting.";

        public ILogger CreateLogger(string loggerName) => new Logger(LogManager.GetLogger(DefaultLoggerName + loggerName));
    }
}
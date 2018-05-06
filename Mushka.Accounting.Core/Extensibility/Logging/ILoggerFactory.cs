namespace Mushka.Accounting.Core.Extensibility.Logging
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger(string loggerName);
    }
}
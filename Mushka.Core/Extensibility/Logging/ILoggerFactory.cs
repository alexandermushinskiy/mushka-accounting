namespace Mushka.Core.Extensibility.Logging
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger(string loggerName);
    }
}
namespace Mushka.Accounting.Core.Extensibility.Logging
{
    public interface ITraceIdentifierProvider
    {
        string TraceIdentifier { get; set; }
    }
}
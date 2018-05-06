namespace Mushka.Accounting.Core.Extensibility.Providers
{
    public interface ICorrelationIdProvider
    {
        string Generate();
    }
}
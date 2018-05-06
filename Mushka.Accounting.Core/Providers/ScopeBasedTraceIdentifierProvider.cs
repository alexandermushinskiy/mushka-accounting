using Mushka.Accounting.Core.Extensibility.Logging;
using Mushka.Accounting.Core.Extensibility.Providers;

namespace Mushka.Accounting.Core.Providers
{
    public class ScopeBasedTraceIdentifierProvider : ITraceIdentifierProvider
    {
        public ScopeBasedTraceIdentifierProvider(ICorrelationIdProvider correlationIdProvider)
        {
            TraceIdentifier = correlationIdProvider.Generate();
        }

        public string TraceIdentifier { get; set; }
    }
}
using System.Threading;
using Mushka.Accounting.Core.Extensibility.Providers;

namespace Mushka.Accounting.Core.Providers
{
    internal class CancellationTokenSourceProvider : ICancellationTokenSourceProvider
    {
        public CancellationTokenSource Get() => new CancellationTokenSource();
    }
}
using System.Threading;
using Mushka.Core.Extensibility.Providers;

namespace Mushka.Core.Providers
{
    internal class CancellationTokenSourceProvider : ICancellationTokenSourceProvider
    {
        public CancellationTokenSource Get() => new CancellationTokenSource();
    }
}
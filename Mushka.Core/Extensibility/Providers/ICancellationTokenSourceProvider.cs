using System.Threading;

namespace Mushka.Core.Extensibility.Providers
{
    public interface ICancellationTokenSourceProvider
    {
        CancellationTokenSource Get();
    }
}
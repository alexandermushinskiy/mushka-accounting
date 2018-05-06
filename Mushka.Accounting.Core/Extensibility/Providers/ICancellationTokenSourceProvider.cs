using System.Threading;

namespace Mushka.Accounting.Core.Extensibility.Providers
{
    public interface ICancellationTokenSourceProvider
    {
        CancellationTokenSource Get();
    }
}
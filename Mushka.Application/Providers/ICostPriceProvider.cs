using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mushka.Application.Providers;

public interface ICostPriceProvider
{
    Task<decimal> CalculateAsync(Guid productId, int productsCount, CancellationToken cancellationToken = default);
}

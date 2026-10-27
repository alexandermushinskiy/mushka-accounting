using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Domain.Extensibility.Repositories;

namespace Mushka.Application.Providers;

public sealed class CostPriceProvider : ICostPriceProvider
{
    private readonly ISupplyRepository _supplyRepository;
    private readonly IOrderRepository _orderRepository;

    public CostPriceProvider(ISupplyRepository supplyRepository, IOrderRepository orderRepository)
    {
        _supplyRepository = supplyRepository;
        _orderRepository = orderRepository;
    }

    public async Task<decimal> CalculateAsync(Guid productId, int productsCount, CancellationToken cancellationToken = default)
    {
        var soldProductCount = await _orderRepository.GetSoldProductCount(productId, cancellationToken);
        var allProductSupplies = await _supplyRepository.GetByProductAsync(productId, cancellationToken);

        var supplyQuantity = 0;
        var costPricesList = new List<decimal>(productsCount);

        foreach (var supplyProduct in allProductSupplies.Where(product => product.ProductId == productId))
        {
            supplyQuantity = supplyProduct.Quantity + supplyQuantity;
            var restProductInSupply = supplyQuantity - soldProductCount;

            if (restProductInSupply <= 0)
            {
                continue;
            }

            if (restProductInSupply >= productsCount)
            {
                costPricesList.AddRange(Enumerable.Repeat(supplyProduct.CostPrice, productsCount));
                break;
            }

            if (restProductInSupply < productsCount)
            {
                productsCount = productsCount - restProductInSupply;
                costPricesList.AddRange(Enumerable.Repeat(supplyProduct.CostPrice, productsCount));
            }
        }

        return costPricesList.Count == 0
            ? 0
            : Math.Round(costPricesList.Sum() / costPricesList.Count, 2);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Service.Extensibility.Providers;

namespace Mushka.Service.Providers
{
    internal class CostPriceProvider : ICostPriceProvider
    {
        private readonly ISupplyRepository supplyRepository;
        private readonly IOrderRepository orderRepository;

        public CostPriceProvider(
            ISupplyRepository supplyRepository,
            IOrderRepository orderRepository)
        {
            this.supplyRepository = supplyRepository;
            this.orderRepository = orderRepository;
        }

        public async Task<decimal> CalculateAsync(Guid productId, int productsCount, CancellationToken cancellationToken = default(CancellationToken))
        {
            var soldProductCount = await orderRepository.GetSoldProductCount(productId, cancellationToken);
            var allProductSupplies = await supplyRepository.GetByProductAsync(productId, cancellationToken);

            var supplyQuantity = 0;
            var costPricesList = new List<decimal>(productsCount);

            foreach (var supplyProduct in allProductSupplies.Where(prod => prod.ProductId == productId))
            {
                supplyQuantity = supplyProduct.Quantity + supplyQuantity;
                var restProductInSupply = supplyQuantity - soldProductCount;

                if (restProductInSupply < 0)
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

            return costPricesList.Sum() / costPricesList.Count;
        }
    }
}
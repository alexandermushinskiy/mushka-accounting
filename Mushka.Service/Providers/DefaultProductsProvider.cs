﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Extensibility.Logging;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Service.Extensibility.Providers;
using Mushka.Service.Services;

namespace Mushka.Service.Providers
{
    internal class DefaultProductsProvider : ServiceBase, IDefaultProductsProvider
    {
        private readonly IProductRepository productRepository;
        private readonly ICostPriceProvider costPriceProvider;

        private static readonly Guid PostcardId = Guid.Parse("07DF9000-2680-43E7-BA2C-D4F0C48A8CB5");
        private static readonly Guid PackageId = Guid.Parse("A6BBAD88-3820-4972-8AE9-FC931A62A1E7");
        private static readonly Guid[] DefaultProductIds = { PostcardId,  PackageId };

        public DefaultProductsProvider(
            IStorage storage,
            ICostPriceProvider costPriceProvider,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.costPriceProvider = costPriceProvider;

            productRepository = storage.GetRepository<IProductRepository>();
        }

        public async Task<OperationResult<IEnumerable<OrderProduct>>> GetOrderDefaultProducts(CancellationToken cancellationToken = default(CancellationToken))
        {
            var products = await GetProductsAsync<OrderProduct>(cancellationToken);
            return OperationResult<IEnumerable<OrderProduct>>.FromResult(products);
        }

        public async Task<OperationResult<IEnumerable<ExhibitionProduct>>> GetExhibitionProducts(CancellationToken cancellationToken = default(CancellationToken))
        {
            var products = await GetProductsAsync<ExhibitionProduct>(cancellationToken);
            return OperationResult<IEnumerable<ExhibitionProduct>>.FromResult(products);
        }

        private async Task<IEnumerable<TEntityProduct>> GetProductsAsync<TEntityProduct>(CancellationToken cancellationToken)
            where TEntityProduct : IEntityProduct, new()
        {
            var products = await productRepository.GetAsync(prod => DefaultProductIds.Contains(prod.Id) && prod.Quantity > 0, cancellationToken);

            return products.Select(async prod => new TEntityProduct
                {
                    ProductId = prod.Id,
                    Product = prod,
                    Quantity = 1,
                    CostPrice = await costPriceProvider.CalculateAsync(prod.Id, prod.Quantity, cancellationToken)
                })
                .Select(x => x.Result);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Extensibility.Logging;
using Mushka.Core.Validation;
using Mushka.Core.Validation.Enums;
using Mushka.Domain.Comparers;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Models;
using Mushka.Domain.Strings;
using Mushka.Service.Extensibility.Dto;
using Mushka.Service.Extensibility.Services;

namespace Mushka.Service.Services
{
    internal class SupplyService : ServiceBase<Supply>, ISupplyService
    {
        private readonly IStorage storage;
        private readonly ISupplyRepository supplyRepository;
        private readonly IProductRepository productRepository;

        public SupplyService(
            IStorage storage,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.storage = storage;

            supplyRepository = storage.GetRepository<ISupplyRepository>();
            productRepository = storage.GetRepository<IProductRepository>();
        }

        public async Task<OperationResult<ItemsWithTotalCount<Supply>>> SearchAsync(
            SearchSuppliesFilter searchSuppliesFilter, CancellationToken cancellationToken = default(CancellationToken))
        {
            var totalCount = await supplyRepository.GetAllCountAsync(cancellationToken);

            IEnumerable<Supply> supplies = (await supplyRepository.GetByFilterAsync(searchSuppliesFilter, cancellationToken)).ToList();

            var result = new ItemsWithTotalCount<Supply>(supplies, totalCount);

            return OperationResult<ItemsWithTotalCount<Supply>>.FromResult(result);
        }

        public async Task<OperationResult<Supply>> GetByIdAsync(Guid supplyId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var supply = await supplyRepository.GetByIdAsync(supplyId, cancellationToken);

            return supply == null
                ? OperationResult<Supply>.FromError(ValidationErrors.SupplyNotFound, ValidationStatusType.NotFound)
                : OperationResult<Supply>.FromResult(supply);
        }

        public async Task<OperationResult> AddAsync(Supply supply, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var supplyProduct in supply.Products)
            {
                var storedProduct = await productRepository.GetByIdAsync(supplyProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    return OperationResult.FromError(ValidationErrors.ProductNotFound, ValidationStatusType.NotFound);
                }
                
                storedProduct.Quantity += supplyProduct.Quantity;
                productRepository.Update(storedProduct);
            }

            supplyRepository.Add(supply);
            await storage.SaveAsync(cancellationToken);

            return OperationResult.Success();
        }

        public async Task<OperationResult> UpdateAsync(Supply supply, CancellationToken cancellationToken = default(CancellationToken))
        {
            var storedSupply = await supplyRepository.GetByIdAsync(supply.Id, cancellationToken);

            if (storedSupply == null)
            {
                return OperationResult.FromError(ValidationErrors.SupplyNotFound, ValidationStatusType.NotFound);
            }

            foreach (var supplyProduct in supply.Products)
            {
                var storedProduct = await productRepository.GetByIdAsync(supplyProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    return OperationResult.FromError(ValidationErrors.ProductNotFound, ValidationStatusType.NotFound);
                }

                var storedSupplyQuantity = storedSupply.Products
                    .FirstOrDefault(p => p.ProductId == storedProduct.Id)?.Quantity ?? 0;

                if (storedSupplyQuantity != supplyProduct.Quantity)
                {
                    storedProduct.Quantity = storedProduct.Quantity - storedSupplyQuantity + supplyProduct.Quantity;
                    productRepository.Update(storedProduct);
                }
            }

            foreach (var removedProduct in storedSupply.Products.Except(supply.Products, new SupplyProductComparer()))
            {
                var storedProduct = await productRepository.GetByIdAsync(removedProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    return OperationResult.FromError(ValidationErrors.ProductNotFound, ValidationStatusType.NotFound);
                }

                storedProduct.Quantity -= removedProduct.Quantity;
                productRepository.Update(storedProduct);
            }

            supplyRepository.Update(supply);
            await storage.SaveAsync(cancellationToken);

            return OperationResult.Success();
        }

        public async Task<OperationResult> DeleteAsync(Guid supplyId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var supply = await supplyRepository.GetByIdAsync(supplyId, cancellationToken);

            if (supply == null)
            {
                return OperationResult.FromError(ValidationErrors.SupplyNotFound, ValidationStatusType.NotFound);
            }

            foreach (var supplyProduct in supply.Products)
            {
                var storedProduct = await productRepository.GetByIdAsync(supplyProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    return OperationResult.FromError(ValidationErrors.ProductNotFound, ValidationStatusType.NotFound);
                }

                storedProduct.Quantity -= supplyProduct.Quantity;
                productRepository.Update(storedProduct);
            }

            supplyRepository.Delete(supply);
            await storage.SaveAsync(cancellationToken);

            return OperationResult.Success();
        }
    }
}
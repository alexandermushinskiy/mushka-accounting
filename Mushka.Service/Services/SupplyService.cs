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

        public async Task<ValidationResponse<IEnumerable<Supply>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<Supply> supplies = (await supplyRepository.GetAllAsync(cancellationToken))
                .OrderBy(supply => supply.Supplier.Name)
                .ThenBy(supply => supply.RequestDate)
                .ToList();

            var message = supplies.Any()
                ? "Supplies were successfully retrieved."
                : "No supplies found.";

            return CreateInfoValidationResponse(supplies, message);
        }

        public async Task<ValidationResponse<Supply>> GetByIdAsync(Guid supplyId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var supply = await supplyRepository.GetByIdAsync(supplyId, cancellationToken);

            return supply == null
                ? CreateWarningValidationResponse($"Supply with id {supplyId} is not found.", ValidationStatusType.NotFound)
                : CreateInfoValidationResponse(supply, $"Supply with id {supplyId} was successfully retrieved.");
        }

        public async Task<ValidationResponse<Supply>> AddAsync(Supply supply, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var supplyProduct in supply.Products)
            {
                var storedProduct = await productRepository.GetByIdAsync(supplyProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    return CreateWarningValidationResponse($"Product with id {supplyProduct.ProductId} is not found.", ValidationStatusType.NotFound);
                }
                
                storedProduct.Quantity += supplyProduct.Quantity;
                productRepository.Update(storedProduct);
            }

            var addedSupply = supplyRepository.Add(supply);
            await storage.SaveAsync(cancellationToken);

            return CreateInfoValidationResponse(addedSupply, $"Supply with id {addedSupply.Id} was successfully added.");
        }

        public async Task<ValidationResponse<Supply>> UpdateAsync(Supply supply, CancellationToken cancellationToken = default(CancellationToken))
        {
            var storedSupply = await supplyRepository.GetByIdAsync(supply.Id, cancellationToken);

            if (storedSupply == null)
            {
                return CreateWarningValidationResponse($"Supply with id {supply.Id} is not found.", ValidationStatusType.NotFound);
            }

            foreach (var supplyProduct in supply.Products)
            {
                var storedProduct = await productRepository.GetByIdAsync(supplyProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    return CreateWarningValidationResponse($"Product with id {supplyProduct.ProductId} is not found.", ValidationStatusType.NotFound);
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
                    return CreateWarningValidationResponse($"Product with id {removedProduct.ProductId} is not found.", ValidationStatusType.NotFound);
                }

                storedProduct.Quantity -= removedProduct.Quantity;
                productRepository.Update(storedProduct);
            }

            var updatedSupply = supplyRepository.Update(supply);
            await storage.SaveAsync(cancellationToken);

            return CreateInfoValidationResponse(updatedSupply, $"Supply with id {supply.Id} was successfully updated.");
        }

        public async Task<ValidationResponse<Supply>> DeleteAsync(Guid supplyId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var supply = await supplyRepository.GetByIdAsync(supplyId, cancellationToken);

            if (supply == null)
            {
                return CreateWarningValidationResponse($"Supply with id {supplyId} is not found.", ValidationStatusType.NotFound);
            }

            foreach (var supplyProduct in supply.Products)
            {
                var storedProduct = await productRepository.GetByIdAsync(supplyProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    return CreateWarningValidationResponse($"Product with id {supplyProduct.ProductId} is not found.", ValidationStatusType.NotFound);
                }

                storedProduct.Quantity -= supplyProduct.Quantity;
                productRepository.Update(storedProduct);
            }

            supplyRepository.Delete(supply);
            await storage.SaveAsync(cancellationToken);

            return CreateInfoValidationResponse(supply, $"Supply with id {supplyId} was successfully deleted.");
        }
    }
}
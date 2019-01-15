using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Extensibility.Logging;
using Mushka.Core.Validation;
using Mushka.Core.Validation.Enums;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Service.Extensibility.Services;

namespace Mushka.Service.Services
{
    internal class SupplyService : ServiceBase<Supply>, ISupplyService
    {
        private readonly ISupplyRepository supplyRepository;
        private readonly IProductRepository productRepository;

        public SupplyService(
            ISupplyRepository supplyRepository,
            IProductRepository productRepository,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.supplyRepository = supplyRepository;
            this.productRepository = productRepository;
        }

        public async Task<ValidationResponse<IEnumerable<Supply>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<Supply> supplies = (await supplyRepository.GetAllAsync(cancellationToken))
                .OrderBy(supply => supply.Supplier.Name)
                .ThenBy(supply => supply.RequestDate)
                .ToList();

            string message = supplies.Any()
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
                await productRepository.UpdateAsync(storedProduct, cancellationToken);
            }

            var addedSupply = await supplyRepository.AddAsync(supply, cancellationToken);
            
            return CreateInfoValidationResponse(addedSupply, $"Supply with id {addedSupply.Id} was successfully added.");
        }

        public Task<ValidationResponse<Supply>> UpdateAsync(Supply supply, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<ValidationResponse<Supply>> DeleteAsync(Guid supplyId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Supply supply = await supplyRepository.GetByIdAsync(supplyId, cancellationToken);

            if (supply == null)
            {
                return CreateWarningValidationResponse($"Supply with id {supplyId} is not found.", ValidationStatusType.NotFound);
            }

            //foreach (var deliveryProduct in delivery.Products)
            //{
            //    var storedProductSize = await productRepository.GetProductSizeAsync(deliveryProduct.ProductId, deliveryProduct.SizeId, cancellationToken);
            //    storedProductSize.Quantity -= deliveryProduct.Quantity;

            //    await productRepository.UpdateProductSize(storedProductSize, cancellationToken);
            //}

            await supplyRepository.DeleteAsync(supply, cancellationToken);

            return CreateInfoValidationResponse(supply, $"Supply with id {supplyId} was successfully deleted.");
        }
    }
}
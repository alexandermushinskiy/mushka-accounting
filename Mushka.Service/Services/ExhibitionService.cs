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
using Mushka.Service.Extensibility.Providers;
using Mushka.Service.Extensibility.Services;

namespace Mushka.Service.Services
{
    internal class ExhibitionService : ServiceBase<Exhibition>, IExhibitionService
    {
        private readonly IStorage storage;
        private readonly IExhibitionRepository exhibitionRepository;
        private readonly IProductRepository productRepository;
        private readonly ICostPriceProvider costPriceProvider;

        public ExhibitionService(
            IStorage storage,
            ICostPriceProvider costPriceProvider,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.storage = storage;
            this.costPriceProvider = costPriceProvider;

            exhibitionRepository = storage.GetRepository<IExhibitionRepository>();
            productRepository = storage.GetRepository<IProductRepository>();
        }

        public async Task<ValidationResponse<IEnumerable<Exhibition>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<Exhibition> exhibitions = (await exhibitionRepository.GetAllAsync(cancellationToken)).ToList();

            var message = exhibitions.Any()
                ? "Exhibitions were successfully retrieved."
                : "No exhibitions found.";

            return CreateInfoValidationResponse(exhibitions, message);
        }

        public async Task<ValidationResponse<Exhibition>> GetByIdAsync(Guid exhibitionId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var exhibition = await exhibitionRepository.GetByIdAsync(exhibitionId, cancellationToken);

            return exhibition == null
                ? CreateWarningValidationResponse($"Exhibition with id {exhibitionId} is not found.", ValidationStatusType.NotFound)
                : CreateInfoValidationResponse(exhibition, $"Exhibition with id {exhibitionId} was successfully retrieved.");
        }

        public async Task<ValidationResponse<IEnumerable<ExhibitionProduct>>> GetDefaultProducts(CancellationToken cancellationToken = default(CancellationToken))
        {
            List<Guid> productIds = new List<Guid> {
                Guid.Parse("07DF9000-2680-43E7-BA2C-D4F0C48A8CB5"), // открытка
                Guid.Parse("A6BBAD88-3820-4972-8AE9-FC931A62A1E7")  // пакет
            };

            var products = (await productRepository.GetAsync(prod => productIds.Contains(prod.Id) && prod.Quantity > 0, cancellationToken))
                .Select(async prod => new ExhibitionProduct
                {
                    ProductId = prod.Id,
                    Product = prod,
                    Quantity = 1,
                    CostPrice = await costPriceProvider.CalculateAsync(prod.Id, prod.Quantity, cancellationToken)
                })
                .Select(x => x.Result);

            return CreateInfoValidationResponse(products, "Default products were successfully retrieved.");
        }

        public async Task<ValidationResponse<Exhibition>> AddAsync(Exhibition exhibition, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var exhibitionProduct in exhibition.Products)
            {
                var storedProduct = await productRepository.GetByIdAsync(exhibitionProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    return CreateWarningValidationResponse($"Product with id {exhibitionProduct.ProductId} is not found.", ValidationStatusType.NotFound);
                }

                if (storedProduct.Quantity < exhibitionProduct.Quantity)
                {
                    return CreateWarningValidationResponse($"Product with id {exhibitionProduct.ProductId} is not enough in stock.");
                }

                storedProduct.Quantity -= exhibitionProduct.Quantity;
                productRepository.Update(storedProduct);
            }
            
            var addedExhibition = exhibitionRepository.Add(exhibition);

            await storage.SaveAsync(cancellationToken);

            return CreateInfoValidationResponse(addedExhibition, $"Exhibition with id {addedExhibition.Id} was successfully added.");
        }

        public async Task<ValidationResponse<Exhibition>> UpdateAsync(Exhibition exhibition, CancellationToken cancellationToken = default(CancellationToken))
        {
            var storedExhibition = await exhibitionRepository.GetByIdAsync(exhibition.Id, cancellationToken);

            if (storedExhibition == null)
            {
                return CreateWarningValidationResponse($"Exhibition with id {exhibition.Id} is not found.", ValidationStatusType.NotFound);
            }

            foreach (var exhibitionProduct in exhibition.Products)
            {
                var storedProduct = await productRepository.GetByIdAsync(exhibitionProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    return CreateWarningValidationResponse($"Product with id {exhibitionProduct.ProductId} is not found.", ValidationStatusType.NotFound);
                }

                var storedOrderQuantity = storedExhibition.Products
                                              .FirstOrDefault(p => p.ProductId == storedProduct.Id)?.Quantity ?? 0;

                if (storedOrderQuantity != exhibitionProduct.Quantity)
                {
                    storedProduct.Quantity = storedProduct.Quantity + storedOrderQuantity - exhibitionProduct.Quantity;
                    productRepository.Update(storedProduct);
                }
            }
            
            var updatedExhibition = exhibitionRepository.Update(exhibition);
            await storage.SaveAsync(cancellationToken);

            return CreateInfoValidationResponse(updatedExhibition, $"Exhibition with id {exhibition.Id} was successfully updated.");
        }

        public async Task<ValidationResponse<Exhibition>> DeleteAsync(Guid exhibitionId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var exhibition = await exhibitionRepository.GetByIdAsync(exhibitionId, cancellationToken);

            if (exhibition == null)
            {
                return CreateWarningValidationResponse($"Exhibition with id {exhibitionId} is not found.", ValidationStatusType.NotFound);
            }

            foreach (var orderProduct in exhibition.Products)
            {
                var storedProduct = await productRepository.GetByIdAsync(orderProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    return CreateWarningValidationResponse($"Product with id {orderProduct.ProductId} is not found.", ValidationStatusType.NotFound);
                }

                storedProduct.Quantity += orderProduct.Quantity;
                productRepository.Update(storedProduct);
            }

            exhibitionRepository.Delete(exhibition);
            await storage.SaveAsync(cancellationToken);

            return CreateInfoValidationResponse(exhibition, $"Exhibition with id {exhibition.Id} was successfully deleted.");
        }
    }
}
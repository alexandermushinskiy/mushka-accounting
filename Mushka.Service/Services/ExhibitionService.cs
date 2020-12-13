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
using Mushka.Domain.Strings;
using Mushka.Service.Extensibility.Services;

namespace Mushka.Service.Services
{
    internal class ExhibitionService : ServiceBase<Exhibition>, IExhibitionService
    {
        private readonly IStorage storage;
        private readonly IExhibitionRepository exhibitionRepository;
        private readonly IProductRepository productRepository;

        public ExhibitionService(
            IStorage storage,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.storage = storage;

            exhibitionRepository = storage.GetRepository<IExhibitionRepository>();
            productRepository = storage.GetRepository<IProductRepository>();
        }

        public async Task<OperationResult<IEnumerable<Exhibition>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<Exhibition> exhibitions = (await exhibitionRepository.GetAllAsync(cancellationToken)).ToList();

            return OperationResult<IEnumerable<Exhibition>>.FromResult(exhibitions);
        }

        public async Task<OperationResult<Exhibition>> GetByIdAsync(Guid exhibitionId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var exhibition = await exhibitionRepository.GetByIdAsync(exhibitionId, cancellationToken);

            return exhibition == null
                ? OperationResult<Exhibition>.FromError(ValidationErrors.ExhibitionNotFound, ValidationStatusType.NotFound)
                : OperationResult<Exhibition>.FromResult(exhibition);
        }

        public async Task<OperationResult<Exhibition>> AddAsync(Exhibition exhibition, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var exhibitionProduct in exhibition.Products)
            {
                var storedProduct = await productRepository.GetByIdAsync(exhibitionProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    return OperationResult<Exhibition>.FromError(ValidationErrors.ExhibitionNotFound, ValidationStatusType.NotFound);
                }

                if (storedProduct.Quantity < exhibitionProduct.Quantity)
                {
                    return OperationResult<Exhibition>.FromError(ValidationErrors.ProductNotEnoughInStock);
                }

                storedProduct.Quantity -= exhibitionProduct.Quantity;
                productRepository.Update(storedProduct);
            }
            
            var addedExhibition = exhibitionRepository.Add(exhibition);

            await storage.SaveAsync(cancellationToken);

            return OperationResult<Exhibition>.FromResult(addedExhibition);
        }

        public async Task<OperationResult<Exhibition>> UpdateAsync(Exhibition exhibition, CancellationToken cancellationToken = default(CancellationToken))
        {
            var storedExhibition = await exhibitionRepository.GetByIdAsync(exhibition.Id, cancellationToken);

            if (storedExhibition == null)
            {
                return OperationResult<Exhibition>.FromError(ValidationErrors.ExhibitionNotFound, ValidationStatusType.NotFound);
            }

            foreach (var exhibitionProduct in exhibition.Products)
            {
                var storedProduct = await productRepository.GetByIdAsync(exhibitionProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    return OperationResult<Exhibition>.FromError(ValidationErrors.ProductNotFound, ValidationStatusType.NotFound);
                }

                var storedOrderQuantity = storedExhibition.Products
                                              .FirstOrDefault(p => p.ProductId == storedProduct.Id)?.Quantity ?? 0;

                if (storedOrderQuantity != exhibitionProduct.Quantity)
                {
                    storedProduct.Quantity = storedProduct.Quantity + storedOrderQuantity - exhibitionProduct.Quantity;
                    productRepository.Update(storedProduct);
                }
            }

            foreach (var removedProduct in storedExhibition.Products.Except(exhibition.Products, new ExhibitionProductComparer()))
            {
                var storedProduct = await productRepository.GetByIdAsync(removedProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    return OperationResult<Exhibition>.FromError(ValidationErrors.ProductNotFound, ValidationStatusType.NotFound);
                }

                storedProduct.Quantity += removedProduct.Quantity;
                productRepository.Update(storedProduct);
            }

            var updatedExhibition = exhibitionRepository.Update(exhibition);
            await storage.SaveAsync(cancellationToken);

            return OperationResult<Exhibition>.FromResult(updatedExhibition);
        }

        public async Task<OperationResult<Exhibition>> DeleteAsync(Guid exhibitionId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var exhibition = await exhibitionRepository.GetByIdAsync(exhibitionId, cancellationToken);

            if (exhibition == null)
            {
                return OperationResult<Exhibition>.FromError(ValidationErrors.ExhibitionNotFound, ValidationStatusType.NotFound);
            }

            foreach (var orderProduct in exhibition.Products)
            {
                var storedProduct = await productRepository.GetByIdAsync(orderProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    return OperationResult<Exhibition>.FromError(ValidationErrors.ProductNotFound, ValidationStatusType.NotFound);
                }

                storedProduct.Quantity += orderProduct.Quantity;
                productRepository.Update(storedProduct);
            }

            var deletedExhibition = exhibitionRepository.Delete(exhibition);
            await storage.SaveAsync(cancellationToken);

            return OperationResult<Exhibition>.FromResult(deletedExhibition);
        }
    }
}
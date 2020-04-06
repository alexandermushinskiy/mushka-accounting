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
using Mushka.Service.Extensibility.ExternalApps;
using Mushka.Service.Extensibility.Services;

namespace Mushka.Service.Services
{
    internal class SupplyService : ServiceBase<Supply>, ISupplyService
    {
        private const string ExportContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private const string ExportFileName = "mushka_export_supply_products.xlsx";

        private readonly IStorage storage;
        private readonly ISupplyRepository supplyRepository;
        private readonly IProductRepository productRepository;
        private readonly ISupplyExcelService supplyExcelService;

        public SupplyService(
            IStorage storage,
            ISupplyExcelService supplyExcelService,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.storage = storage;
            this.supplyExcelService = supplyExcelService;

            supplyRepository = storage.GetRepository<ISupplyRepository>();
            productRepository = storage.GetRepository<IProductRepository>();
        }

        public async Task<ValidationResponse<ItemsWithTotalCount<Supply>>> GetByFilterAsync(SuppliesFiltersModel suppliesFiltersModel, CancellationToken cancellationToken = default(CancellationToken))
        {
            var totalCount = await supplyRepository.GetAllCountAsync(cancellationToken);

            IEnumerable<Supply> supplies = (await supplyRepository.GetByFilterAsync(suppliesFiltersModel, cancellationToken)).ToList();

            var result = new ItemsWithTotalCount<Supply>(supplies, totalCount);

            var message = supplies.Any()
                ? ValidationMessages.SuppliesRetrieved
                : ValidationMessages.NoSuppliesFound;

            return CreateInfoValidationResponse(result, message);
        }

        public async Task<ValidationResponse<Supply>> GetByIdAsync(Guid supplyId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var supply = await supplyRepository.GetByIdAsync(supplyId, cancellationToken);

            return supply == null
                ? CreateErrorValidationResponse(ValidationErrors.SupplyNotFound, ValidationStatusType.NotFound)
                : CreateInfoValidationResponse(supply, ValidationMessages.SuppliesRetrieved);
        }

        public async Task<ValidationResponse<Supply>> AddAsync(Supply supply, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach (var supplyProduct in supply.Products)
            {
                var storedProduct = await productRepository.GetByIdAsync(supplyProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    return CreateErrorValidationResponse(ValidationErrors.ProductNotFound, ValidationStatusType.NotFound);
                }
                
                storedProduct.Quantity += supplyProduct.Quantity;
                productRepository.Update(storedProduct);
            }

            var addedSupply = supplyRepository.Add(supply);
            await storage.SaveAsync(cancellationToken);

            return CreateInfoValidationResponse(addedSupply, ValidationMessages.SupplyAdded);
        }

        public async Task<ValidationResponse<Supply>> UpdateAsync(Supply supply, CancellationToken cancellationToken = default(CancellationToken))
        {
            var storedSupply = await supplyRepository.GetByIdAsync(supply.Id, cancellationToken);

            if (storedSupply == null)
            {
                return CreateErrorValidationResponse(ValidationErrors.SupplyNotFound, ValidationStatusType.NotFound);
            }

            foreach (var supplyProduct in supply.Products)
            {
                var storedProduct = await productRepository.GetByIdAsync(supplyProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    return CreateErrorValidationResponse(ValidationErrors.ProductNotFound, ValidationStatusType.NotFound);
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
                    return CreateErrorValidationResponse(ValidationErrors.ProductNotFound, ValidationStatusType.NotFound);
                }

                storedProduct.Quantity -= removedProduct.Quantity;
                productRepository.Update(storedProduct);
            }

            var updatedSupply = supplyRepository.Update(supply);
            await storage.SaveAsync(cancellationToken);

            return CreateInfoValidationResponse(updatedSupply, ValidationMessages.SupplyUpdated);
        }

        public async Task<ValidationResponse<Supply>> DeleteAsync(Guid supplyId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var supply = await supplyRepository.GetByIdAsync(supplyId, cancellationToken);

            if (supply == null)
            {
                return CreateErrorValidationResponse(ValidationErrors.SupplyNotFound, ValidationStatusType.NotFound);
            }

            foreach (var supplyProduct in supply.Products)
            {
                var storedProduct = await productRepository.GetByIdAsync(supplyProduct.ProductId, cancellationToken);

                if (storedProduct == null)
                {
                    return CreateErrorValidationResponse(ValidationErrors.ProductNotFound, ValidationStatusType.NotFound);
                }

                storedProduct.Quantity -= supplyProduct.Quantity;
                productRepository.Update(storedProduct);
            }

            supplyRepository.Delete(supply);
            await storage.SaveAsync(cancellationToken);

            return CreateInfoValidationResponse(supply, ValidationMessages.SupplyDeleted);
        }
        
        public async Task<ValidationResponse<ExportedFile>> ExportAsync(IEnumerable<Guid> supplyIds, IEnumerable<Guid> productIds, CancellationToken cancellationToken = default(CancellationToken))
        {
            var includes = new[]
            {
                nameof(Supply.Supplier),
                nameof(Supply.Products)
            };

            IEnumerable<Supply> supplies =
                (await supplyRepository.GetAsync(sup => supplyIds.Contains(sup.Id), includes, cancellationToken))
                    .OrderBy(supply => supply.Supplier.Name)
                    .ThenBy(supply => supply.RequestDate)
                    .ToList();

            var productIdsList = productIds.ToList();
            if (productIdsList.Count == 0)
            {
                productIdsList = supplies.SelectMany(sup => sup.Products)
                    .Select(supProd => supProd.ProductId)
                    .ToList();
            }

            var products = (productIdsList.Count == 0
                ? await productRepository.GetAllAsync(cancellationToken)
                : await productRepository.GetAsync(prod => productIdsList.Contains(prod.Id), cancellationToken))
                    .OrderBy(prod => prod.Name)
                    .ThenBy(prod => prod.VendorCode)
                    .ToList();
            
            var fileContent = supplyExcelService.ExportSupplies(supplies, products);
            var exportedFile = new ExportedFile(ExportFileName, ExportContentType, fileContent);

            return CreateInfoValidationResponse(exportedFile, ValidationMessages.SupplyExported);
        }
    }
}
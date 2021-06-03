using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Extensibility.Logging;
using Mushka.Core.Validation;
using Mushka.Core.Validation.Enums;
using Mushka.Domain.Dto;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;
using Mushka.Service.Extensibility.Dto;
using Mushka.Service.Extensibility.ExternalApps;
using Mushka.Service.Extensibility.Providers;
using Mushka.Service.Extensibility.Services;

namespace Mushka.Service.Services
{
    internal class ProductService : ServiceBase<Product>, IProductService
    {
        private const string ExportContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private const string ExportFileName = "mushka_export_products.xlsx";

        private readonly IStorage storage;
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ICostPriceProvider costPriceProvider;
        private readonly IExcelService excelService;

        public ProductService(
            IStorage storage,
            ICostPriceProvider costPriceProvider,
            IExcelService excelService,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.storage = storage;
            this.costPriceProvider = costPriceProvider;
            this.excelService = excelService;

            productRepository = storage.GetRepository<IProductRepository>();
            categoryRepository = storage.GetRepository<ICategoryRepository>();
        }

        public async Task<OperationResult<IEnumerable<Product>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<Product> products = (await productRepository.GetAllAsync(cancellationToken))
                .OrderBy(product => product.Name)
                .ToList();

            return OperationResult<IEnumerable<Product>>.FromResult(products);
        }

        public async Task<OperationResult<IEnumerable<Product>>> GetInStockAsync(bool inStock, CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<Product> products = (inStock 
                    ? await productRepository.GetAsync(prod => prod.Quantity > 0, cancellationToken)
                    : await productRepository.GetAllAsync(cancellationToken))
                .OrderBy(product => product.Name)
                .ToList();

            return OperationResult<IEnumerable<Product>>.FromResult(products);
        }

        public async Task<OperationResult<Product>> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var product = await productRepository.GetByIdAsync(productId, cancellationToken);

            return product == null
                ? OperationResult<Product>.FromError(ValidationErrors.ProductNotFound, ValidationStatusType.NotFound)
                : OperationResult<Product>.FromResult(product);
        }

        public async Task<OperationResult<IEnumerable<Product>>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!await categoryRepository.IsExistAsync(category => category.Id == categoryId, cancellationToken))
            {
                CreateErrorValidationResponse($"Category with id {categoryId} is not found.", ValidationStatusType.NotFound);
            }

            IEnumerable<Product> products = (await productRepository.GetByCategoryId(categoryId, cancellationToken))
                .OrderBy(product => product.Name)
                .ToList();

            return OperationResult<IEnumerable<Product>>.FromResult(products);
        }

        public async Task<OperationResult<IEnumerable<Product>>> GetByCriteriaAsync(string criteria, CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<Product> products = (await productRepository.GetAsync(prod =>
                    prod.Name.ToUpper().Contains(criteria.ToUpper()) || prod.VendorCode.ToUpper().Contains(criteria.ToUpper()), cancellationToken))
                .OrderBy(product => product.Name)
                .ToList();

            return OperationResult<IEnumerable<Product>>.FromResult(products);
        }

        public async Task<OperationResult<ProductCostPrice>> GetCostPriceAsync(Guid productId, int productsCount, CancellationToken cancellationToken = default(CancellationToken))
        {
            var product = await productRepository.GetByIdAsync(productId, cancellationToken);

            if (product == null)
            {
                return OperationResult<ProductCostPrice>.FromError(ValidationErrors.ProductNotFound, ValidationStatusType.NotFound);
            }

            var costPrice = await costPriceProvider.CalculateAsync(productId, productsCount, cancellationToken);

            return OperationResult<ProductCostPrice>.FromResult(new ProductCostPrice(costPrice));
        }

        public async Task<OperationResult> AddAsync(Product product, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!await categoryRepository.IsExistAsync(category => category.Id == product.CategoryId, cancellationToken))
            {
                return OperationResult.FromError(ValidationErrors.CategoryNotFound, ValidationStatusType.NotFound);
            }
            
            if (await productRepository.IsExistAsync(prod => prod.VendorCode == product.VendorCode, cancellationToken))
            {
                return OperationResult.FromError(ValidationErrors.ProductWithVendorCodeExist);
            }

            productRepository.Add(product);
            await storage.SaveAsync(cancellationToken);

            return OperationResult.Success();
        }

        public async Task<OperationResult> UpdateAsync(Product product, CancellationToken cancellationToken = default(CancellationToken))
        {
            var productToUpdate = await productRepository.GetByIdAsync(product.Id, cancellationToken);

            if (productToUpdate == null)
            {
                return OperationResult.FromError(ValidationErrors.ProductNotFound, ValidationStatusType.NotFound);
            }

            if (await productRepository.IsExistAsync(prod => prod.Id != product.Id && prod.VendorCode == product.VendorCode, cancellationToken))
            {
                return OperationResult.FromError(ValidationErrors.ProductWithVendorCodeExist);
            }

            product.Quantity = productToUpdate.Quantity;

            productRepository.Update(product);
            await storage.SaveAsync(cancellationToken);

            return OperationResult.Success();
        }

        public async Task<OperationResult> DeleteAsync(Guid productId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var product = await productRepository.GetByIdAsync(productId, cancellationToken);

            if (product == null)
            {
                return OperationResult.FromError(ValidationErrors.ProductNotFound, ValidationStatusType.NotFound);
            }

            productRepository.Delete(product);
            await storage.SaveAsync(cancellationToken);

            return OperationResult.Success();
        }

        public async Task<OperationResult<IEnumerable<Size>>> GetSizesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var sizes = (await productRepository.GetSizesAsync(cancellationToken)).ToList();

            return OperationResult<IEnumerable<Size>>.FromResult(sizes);
        }

        public async Task<OperationResult<ExportedFile>> ExportAsync(string title, IEnumerable<Guid> productIds, CancellationToken cancellationToken = default(CancellationToken))
        {
            var products = await productRepository.GetForExportAsync(prod => productIds.Contains(prod.Id), cancellationToken);

            var fileContent = excelService.ExportProducts(title, products);
            var exportedFile = new ExportedFile(ExportFileName, ExportContentType, fileContent);

            return OperationResult<ExportedFile>.FromResult(exportedFile);
        }
     }
}
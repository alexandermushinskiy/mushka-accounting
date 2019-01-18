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
    internal class ProductService : ServiceBase<Product>, IProductService
    {
        private readonly IStorage storage;
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ICostPriceProvider costPriceProvider;

        public ProductService(
            IStorage storage,
            ICostPriceProvider costPriceProvider,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.storage = storage;
            this.costPriceProvider = costPriceProvider;

            productRepository = storage.GetRepository<IProductRepository>();
            categoryRepository = storage.GetRepository<ICategoryRepository>();
        }

        public async Task<ValidationResponse<IEnumerable<Product>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<Product> products = (await productRepository.GetAllAsync(cancellationToken))
                .OrderBy(product => product.Name)
                .ToList();

            var message = products.Any()
                ? "Products were successfully retrieved."
                : "No products found.";

            return CreateInfoValidationResponse(products, message);
        }

        public async Task<ValidationResponse<IEnumerable<Product>>> GetInStockAsync(bool inStock, CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<Product> products = (inStock 
                    ? await productRepository.GetAsync(prod => prod.Quantity > 0, cancellationToken)
                    : await productRepository.GetAllAsync(cancellationToken))
                .OrderBy(product => product.Name)
                .ToList();

            var message = products.Any()
                ? "Products in stock were successfully retrieved."
                : "No products found in stock.";

            return CreateInfoValidationResponse(products, message);
        }

        public async Task<ValidationResponse<Product>> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var product = await productRepository.GetByIdAsync(productId, cancellationToken);

            return product == null
                ? CreateWarningValidationResponse($"Product with id {productId} is not found.", ValidationStatusType.NotFound)
                : CreateInfoValidationResponse(product, $"Product with id {product.Id} was successfully retrieved.");
        }

        public async Task<ValidationResponse<IEnumerable<Product>>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!await categoryRepository.IsExistAsync(category => category.Id == categoryId, cancellationToken))
            {
                CreateWarningValidationResponse($"Category with id {categoryId} is not found.", ValidationStatusType.NotFound);
            }

            IEnumerable<Product> products = (await productRepository.GetByCategoryId(categoryId, cancellationToken))
                .OrderBy(product => product.Name)
                .ToList();

            var message = products.Any()
                ? $"Products were successfully retrieved for category {categoryId}."
                : $"No products for category {categoryId}.";

            return CreateInfoValidationResponse(products, message);
        }

        public async Task<ValidationResponse<IEnumerable<Product>>> GetByCriteriaAsync(string criteria, CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<Product> products = (await productRepository.GetAsync(prod =>
                    prod.Name.ToUpper().Contains(criteria.ToUpper()) || prod.VendorCode.ToUpper().Contains(criteria.ToUpper()), cancellationToken))
                .OrderBy(product => product.Name)
                .ToList();

            var message = products.Any()
                ? $"Products were successfully retrieved by criteria {criteria}."
                : $"No products for criteria {criteria}.";

            return CreateInfoValidationResponse(products, message);
        }

        public async Task<ValidationResponse<decimal>> GetCostPriceAsync(Guid productId, int productsCount, CancellationToken cancellationToken = default(CancellationToken))
        {
            var product = await productRepository.GetByIdAsync(productId, cancellationToken);

            //if (product == null)
            //{
            //    return CreateWarningValidationResponse<decimal>($"Product with id {productId} is not found.", ValidationStatusType.NotFound);
            //}

            var costPrice = await costPriceProvider.CalculateAsync(productId, productsCount, cancellationToken);

            return CreateInfoValidationResponse(costPrice, $"Cost price for product with id {product.Id} was successfully retrieved.");
        }

        public async Task<ValidationResponse<Product>> AddAsync(Product product, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!await categoryRepository.IsExistAsync(category => category.Id == product.CategoryId, cancellationToken))
            {
                return CreateWarningValidationResponse($"Category with id {product.CategoryId} is not found.", ValidationStatusType.NotFound);
            }

            //if (await productRepository.IsExistAsync(prod => prod.Name == product.Name && prod.SizeId == product.SizeId, cancellationToken))
            //{
            //    return CreateWarningValidationResponse($"Product with the name {product.Name} is already existed.");
            //}

            if (await productRepository.IsExistAsync(prod => prod.VendorCode == product.VendorCode, cancellationToken))
            {
                return CreateWarningValidationResponse($"Product with the vendor code {product.VendorCode} is already existed.");
            }

            productRepository.Add(product);
            await storage.SaveAsync(cancellationToken);

            var addedProduct = await productRepository.GetByIdAsync(product.Id, cancellationToken);

            return CreateInfoValidationResponse(addedProduct, $"Product with id {product.Id} was successfully created.");
        }

        public async Task<ValidationResponse<Product>> UpdateAsync(Product product, CancellationToken cancellationToken = default(CancellationToken))
        {
            var productToUpdate = await productRepository.GetByIdAsync(product.Id, cancellationToken);

            if (productToUpdate == null)
            {
                return CreateWarningValidationResponse($"Product with id {product.Id} is not found.", ValidationStatusType.NotFound);
            }

            if (productRepository.Get(prod => prod.Id != product.Id && prod.Name == product.Name).Any())
            {
                return CreateWarningValidationResponse($"Product with the name {product.Name} is already exist.");
            }

            productRepository.Update(product);
            await storage.SaveAsync(cancellationToken);
            var updatedProduct = await productRepository.GetByIdAsync(product.Id, cancellationToken);

            return CreateInfoValidationResponse(updatedProduct, $"Product with id {product.Id} was successfully updated.");
        }

        public async Task<ValidationResponse<Product>> DeleteAsync(Guid productId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var product = await productRepository.GetByIdAsync(productId, cancellationToken);

            if (product == null)
            {
                return CreateWarningValidationResponse($"Product with id {productId} is not found.", ValidationStatusType.NotFound);
            }

            productRepository.Delete(product);
            await storage.SaveAsync(cancellationToken);

            return CreateInfoValidationResponse(product, $"Product with id {product.Id} was successfully deleted.");
        }

        public async Task<ValidationResponse<IEnumerable<Size>>> GetSizesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var sizes = (await productRepository.GetSizesAsync(cancellationToken)).ToList();

            string message = sizes.Any()
                ? "Sizes were successfully retrieved."
                : "No sizes found.";

            return CreateInfoValidationResponse<IEnumerable<Size>>(sizes, message);
        }
    }
}
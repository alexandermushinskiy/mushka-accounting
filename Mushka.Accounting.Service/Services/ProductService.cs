﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Accounting.Core.Extensibility.Logging;
using Mushka.Accounting.Core.Extensibility.Validation;
using Mushka.Accounting.Core.Validation;
using Mushka.Accounting.Core.Validation.Enums;
using Mushka.Accounting.Domain.Entities;
using Mushka.Accounting.Domain.Extensibility.Repositories;
using Mushka.Accounting.Service.Extensibility.Services;

namespace Mushka.Accounting.Service.Services
{
    internal class ProductService : ServiceBase<Product>, IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;

        public ProductService(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
        }

        public async Task<ValidationResponse<IEnumerable<Product>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<Product> products = (await productRepository.GetAllAsync(cancellationToken))
                .OrderBy(product => product.Name)
                .ToList();

            string message = products.Any()
                ? "Products were successfully retrieved."
                : "No products found.";

            return CreateInfoValidationResponse(products, message);
        }

        public async Task<ValidationResponse<IEnumerable<Product>>> GetByCategoryAsync(Guid categoryId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Category category = await categoryRepository.GetByIdAsync(categoryId, cancellationToken);

            if (category == null)
            {
                IValidationResult validationResult = ValidationResult.CreateWarning(
                    $"Category with id {categoryId} is not found.", ValidationStatusType.NotFound);
                return new ValidationResponse<IEnumerable<Product>>(null, validationResult);
            }

            IEnumerable<Product> products = productRepository.Get(prod => prod.Category.Id == categoryId);

            string message = products.Any()
                ? $"Products were successfully retrieved for category {categoryId}."
                : $"No products for category {categoryId}.";

            return CreateInfoValidationResponse(products, message);
        }

        public async Task<ValidationResponse<Product>> GetByIdAsync(Guid productId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Product product = await productRepository.GetByIdAsync(productId, cancellationToken);

            return product == null
                ? CreateWarningValidationResponse($"Product with id {productId} is not found.", ValidationStatusType.NotFound)
                : CreateInfoValidationResponse(product, $"Product {product.Id} was successfully retrieved.");
        }

        public async Task<ValidationResponse<Product>> AddAsync(Product product, CancellationToken cancellationToken = default(CancellationToken))
        {
            bool isExistProductName = productRepository.Get(prod => prod.Name == product.Name).Any();

            if (isExistProductName)
            {
                return CreateWarningValidationResponse($"Product with the name {product.Name} is already existed.");
            }

            Product addedProduct = await productRepository.AddAsync(product, cancellationToken);

            return CreateInfoValidationResponse(addedProduct, $"Product {product.Id} was successfully created.");
        }

        public async Task<ValidationResponse<Product>> UpdateAsync(Product product, CancellationToken cancellationToken = default(CancellationToken))
        {
            Product productToUpdate = await productRepository.GetByIdAsync(product.Id, cancellationToken);

            if (productToUpdate == null)
            {
                return CreateWarningValidationResponse($"Product with id {product.Id} is not found.", ValidationStatusType.NotFound);
            }

            if (productRepository.Get(prod => prod.Id != product.Id && prod.Name == product.Name).Any())
            {
                return CreateWarningValidationResponse($"Product with the name {product.Name} is already exist.");
            }

            Product updatedProduct = await productRepository.UpdateAsync(product, cancellationToken);

            return CreateInfoValidationResponse(updatedProduct, $"Product {product.Id} was successfully updated.");
        }

        public async Task<ValidationResponse<Product>> DeleteAsync(Guid productId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Product product = await productRepository.GetByIdAsync(productId, cancellationToken);

            if (product == null)
            {
                return CreateWarningValidationResponse($"Product with id {productId} is not found.", ValidationStatusType.NotFound);
            }

            await productRepository.DeleteAsync(product, cancellationToken);

            return CreateInfoValidationResponse(product, $"Product {product.Id} was successfully deleted.");
        }
    }
}
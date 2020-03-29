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
    internal class CategoryService : ServiceBase<Category>, ICategoryService
    {
        private readonly IStorage storage;
        private readonly ICategoryRepository categoryRepository;
        private readonly IProductRepository productRepository;

        public CategoryService(
            IStorage storage,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.storage = storage;

            categoryRepository = storage.GetRepository<ICategoryRepository>();
            productRepository = storage.GetRepository<IProductRepository>();
        }

        public async Task<ValidationResponse<IEnumerable<Category>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<Category> categories = (await categoryRepository.GetAllAsync(cancellationToken))
                .OrderBy(category => category.Order)
                .ToList();

            var message = categories.Any()
                ? "Categories were successfully retrieved."
                : "No categories found.";

            return CreateInfoValidationResponse(categories, message);
        }

        public async Task<ValidationResponse<Category>> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var category = await categoryRepository.GetByIdAsync(categoryId, cancellationToken);

            return category == null
                ? CreateErrorValidationResponse($"Category with id {categoryId} is not found.", ValidationStatusType.NotFound)
                : CreateInfoValidationResponse(category, $"Category with id {category.Id} was successfully retrieved.");
        }

        public async Task<ValidationResponse<Category>> AddAsync(Category category, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (await categoryRepository.IsExistAsync(cat => cat.Name == category.Name, cancellationToken))
            {
                return CreateErrorValidationResponse($"Category with name {category.Name} is already exist.");
            }
            
            var addedCategory = categoryRepository.Add(category);
            await storage.SaveAsync(cancellationToken);

            return CreateInfoValidationResponse(addedCategory, $"Category with id {category.Id} was successfully added.");
        }

        public async Task<ValidationResponse<Category>> UpdateAsync(Category category, CancellationToken cancellationToken = default(CancellationToken))
        {
            var categoryToUpdate = await categoryRepository.GetByIdAsync(category.Id, cancellationToken);

            if (categoryToUpdate == null)
            {
                return CreateErrorValidationResponse($"Category with id {category.Id} is not found.", ValidationStatusType.NotFound);
            }

            if (await categoryRepository.IsExistAsync(cat => cat.Id != category.Id && cat.Name == category.Name, cancellationToken))
            {
                return CreateErrorValidationResponse($"Category with name {category.Name} is already exist.");
            }

            category.Order = categoryToUpdate.Order;

            var updatedCategory = categoryRepository.Update(category);
            await storage.SaveAsync(cancellationToken);

            return CreateInfoValidationResponse(updatedCategory, $"Category with id {category.Id} was successfully updated.");
        }

        public async Task<ValidationResponse<Category>> DeleteAsync(Guid categoryId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var category = await categoryRepository.GetByIdAsync(categoryId, cancellationToken);

            if (category == null)
            {
                return CreateErrorValidationResponse($"Category with id {categoryId} is not found.", ValidationStatusType.NotFound);
            }

            if ((await productRepository.GetByCategoryId(categoryId, cancellationToken)).Any())
            {
                return CreateErrorValidationResponse($"Category with id {categoryId} contains products.");
            }

            categoryRepository.Delete(category);
            await storage.SaveAsync(cancellationToken);

            return CreateInfoValidationResponse(category, $"Category with id {category.Id} was successfully deleted.");
        }
    }
}
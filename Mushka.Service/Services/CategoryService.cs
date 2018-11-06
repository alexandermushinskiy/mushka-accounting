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
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(
            ICategoryRepository categoryRepository,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.categoryRepository = categoryRepository;
        }

        public async Task<ValidationResponse<IEnumerable<Category>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<Category> categories = (await categoryRepository.GetAllAsync(cancellationToken))
                .OrderBy(category => category.Name)
                .ToList();

            string message = categories.Any()
                ? "Categories were successfully retrieved."
                : "No categories found.";

            return CreateInfoValidationResponse(categories, message);
        }

        public async Task<ValidationResponse<Category>> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Category category = await categoryRepository.GetByIdAsync(categoryId, cancellationToken);

            return category == null
                ? CreateWarningValidationResponse($"Category with id {categoryId} is not found.", ValidationStatusType.NotFound)
                : CreateInfoValidationResponse(category, $"Category with id {category.Id} was successfully retrieved.");
        }

        public async Task<ValidationResponse<Category>> AddAsync(Category category, CancellationToken cancellationToken = default(CancellationToken))
        {
            bool isExistCategoryName = categoryRepository.Get(cat => cat.Name == category.Name).Any();

            if (isExistCategoryName)
            {
                return CreateWarningValidationResponse($"Category with the name {category.Name} is already existed.");
            }

            Category addedCategory = await categoryRepository.AddAsync(category, cancellationToken);

            return CreateInfoValidationResponse(addedCategory, $"Category with id {category.Id} was successfully created.");
        }

        public async Task<ValidationResponse<Category>> UpdateAsync(Category category, CancellationToken cancellationToken = default(CancellationToken))
        {
            Category categoryToUpdate = await categoryRepository.GetByIdAsync(category.Id, cancellationToken);

            if (categoryToUpdate == null)
            {
                return CreateWarningValidationResponse($"Category with id {category.Id} is not found.", ValidationStatusType.NotFound);
            }

            if (categoryRepository.Get(cat => cat.Id != category.Id && cat.Name == category.Name).Any())
            {
                return CreateWarningValidationResponse($"Category with the name {category.Name} is already exist.");
            }

            Category updatedCategory = await categoryRepository.UpdateAsync(category, cancellationToken);

            return CreateInfoValidationResponse(updatedCategory, $"Category with id {category.Id} was successfully updated.");
        }

        public async Task<ValidationResponse<Category>> DeleteAsync(Guid categoryId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Category category = await categoryRepository.GetByIdAsync(categoryId, cancellationToken);

            if (category == null)
            {
                return CreateWarningValidationResponse($"Category with id {categoryId} is not found.", ValidationStatusType.NotFound);
            }

            await categoryRepository.DeleteAsync(category, cancellationToken);

            return CreateInfoValidationResponse(category, $"Category with id {category.Id} was successfully deleted.");
        }
    }
}
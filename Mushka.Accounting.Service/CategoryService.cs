using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Accounting.Core.Validation;
using Mushka.Accounting.Core.Validation.Enums;
using Mushka.Accounting.Domain.Entities;
using Mushka.Accounting.Domain.Extensibility.Repositories;
using Mushka.Accounting.Service.Extensibility;

namespace Mushka.Accounting.Service
{
    internal class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
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

            return new ValidationResponse<IEnumerable<Category>>(categories, ValidationResult.CreateInfo(message));
        }

        public async Task<ValidationResponse<Category>> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Category category = await categoryRepository.GetByIdAsync(categoryId, cancellationToken);

            return category == null
                ? CreateWarningValidationResponse($"Category with id {categoryId} is not found.", ValidationStatusType.NotFound)
                : new ValidationResponse<Category>(category, ValidationResult.CreateInfo($"Category {category.Id} was successfully retrieved."));
        }

        public async Task<ValidationResponse<Category>> AddAsync(Category category, CancellationToken cancellationToken = default(CancellationToken))
        {
            bool isExistCategoryName = categoryRepository.Get(cat => cat.Name == category.Name).Any();

            if (isExistCategoryName)
            {
                return CreateWarningValidationResponse($"Category with the name {category.Name} is already existed.");
            }

            Category addedCategory = await categoryRepository.AddAsync(category, cancellationToken);

            return new ValidationResponse<Category>(addedCategory, ValidationResult.CreateInfo($"Category {category.Id} was successfully created."));
        }

        public Task<ValidationResponse<Category>> UpdateAsync(Category category, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<ValidationResponse<Category>> DeleteAsync(Guid categoryId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Category category = await categoryRepository.GetByIdAsync(categoryId, cancellationToken);

            if (category == null)
            {
                return CreateWarningValidationResponse($"Category with id {categoryId} is not found.", ValidationStatusType.NotFound);
            }

            await categoryRepository.DeleteAsync(category, cancellationToken);

            return new ValidationResponse<Category>(category, ValidationResult.CreateInfo($"Category {category.Id} was successfully deleted."));
        }

        private static ValidationResponse<Category> CreateWarningValidationResponse(
            string message,
            ValidationStatusType validationStatus = ValidationStatusType.BadOperation) =>
            new ValidationResponse<Category>(null, ValidationResult.CreateWarning(message, validationStatus));
    }
}
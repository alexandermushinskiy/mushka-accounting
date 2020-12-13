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
using Mushka.Domain.Strings;
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

        public async Task<OperationResult<IEnumerable<Category>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<Category> categories = (await categoryRepository.GetAllAsync(cancellationToken))
                .OrderBy(category => category.Order)
                .ToList();

            return OperationResult<IEnumerable<Category>>.FromResult(categories);
        }

        public async Task<OperationResult<Category>> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var category = await categoryRepository.GetByIdAsync(categoryId, cancellationToken);

            return category == null
                ? OperationResult<Category>.FromError(ValidationErrors.CategoryNotFound, ValidationStatusType.NotFound)
                : OperationResult<Category>.FromResult(category);
        }

        public async Task<OperationResult<Category>> AddAsync(Category category, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (await categoryRepository.IsExistAsync(cat => cat.Name == category.Name, cancellationToken))
            {
                return OperationResult<Category>.FromError(ValidationErrors.CategoryWithNameExist);
            }
            
            var addedCategory = categoryRepository.Add(category);
            await storage.SaveAsync(cancellationToken);

            return OperationResult<Category>.FromResult(addedCategory);
        }

        public async Task<OperationResult<Category>> UpdateAsync(Category category, CancellationToken cancellationToken = default(CancellationToken))
        {
            var categoryToUpdate = await categoryRepository.GetByIdAsync(category.Id, cancellationToken);

            if (categoryToUpdate == null)
            {
                return OperationResult<Category>.FromError(ValidationErrors.CategoryNotFound, ValidationStatusType.NotFound);
            }

            if (await categoryRepository.IsExistAsync(cat => cat.Id != category.Id && cat.Name == category.Name, cancellationToken))
            {
                return OperationResult<Category>.FromError(ValidationErrors.CategoryWithNameExist);
            }

            category.Order = categoryToUpdate.Order;

            var updatedCategory = categoryRepository.Update(category);
            await storage.SaveAsync(cancellationToken);

            return OperationResult<Category>.FromResult(updatedCategory);
        }

        public async Task<OperationResult<Category>> DeleteAsync(Guid categoryId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var category = await categoryRepository.GetByIdAsync(categoryId, cancellationToken);

            if (category == null)
            {
                return OperationResult<Category>.FromError(ValidationErrors.CategoryNotFound, ValidationStatusType.NotFound);
            }

            if ((await productRepository.GetByCategoryId(categoryId, cancellationToken)).Any())
            {
                return OperationResult<Category>.FromError(ValidationErrors.CategoryCannotBeDeletedWithProducts);
            }

            var deletedCategory = categoryRepository.Delete(category);
            await storage.SaveAsync(cancellationToken);

            return OperationResult<Category>.FromResult(deletedCategory);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Accounting.Core.Validation;
using Mushka.Accounting.Domain.Entities;

namespace Mushka.Accounting.Service.Extensibility
{
    public interface ICategoryService
    {
        Task<ValidationResponse<IEnumerable<Category>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<Category>> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<Category>> AddAsync(Category category, CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<Category>> UpdateAsync(Category category, CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<Category>> DeleteAsync(Guid categorywId, CancellationToken cancellationToken = default(CancellationToken));
    }
}
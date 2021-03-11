using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;

namespace Mushka.Service.Extensibility.Services
{
    public interface IExpenseService : IServiceBase<Expense>
    {
        Task<OperationResult<IEnumerable<Expense>>> SearchAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<OperationResult<IEnumerable<Expense>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
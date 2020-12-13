using System;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;

namespace Mushka.Service.Extensibility.Providers
{
    public interface IOrderCustomerProvider
    {
        Task<OperationResult<Customer>> GetCustomerForNewOrderAsync(Customer customer, CancellationToken cancellationToken = default(CancellationToken));

        Task<OperationResult<Customer>> GetCustomerForExistingOrderAsync(Guid storedCustomerId, Customer customer, CancellationToken cancellationToken = default(CancellationToken));
    }
}
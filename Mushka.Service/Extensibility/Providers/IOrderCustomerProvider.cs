using System;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;

namespace Mushka.Service.Extensibility.Providers
{
    public interface IOrderCustomerProvider
    {
        Task<ValidationResponse<Customer>> GetCustomerForNewOrderAsync(Customer customer, CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<Customer>> GetCustomerForExistingOrderAsync(Guid storedCustomerId, Customer customer, CancellationToken cancellationToken = default(CancellationToken));
    }
}
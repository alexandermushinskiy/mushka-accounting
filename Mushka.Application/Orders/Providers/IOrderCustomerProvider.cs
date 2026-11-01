using System;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Domain.Entities;

namespace Mushka.Application.Orders.Providers;

public interface IOrderCustomerProvider
{
    Task<Customer> GetCustomerForNewOrderAsync(Customer customer, CancellationToken cancellationToken);

    Task<Customer> GetCustomerForExistingOrderAsync(Guid storedCustomerId, Customer customer, CancellationToken cancellationToken);
}

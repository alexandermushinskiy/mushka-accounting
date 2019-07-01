using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Domain.Entities;

namespace Mushka.Domain.Extensibility.Repositories
{
    public interface ICustomerRepository : IRepositoryBase<Customer>
    {
        Task<Customer> GetByOrderDetailsAsync(Customer orderCustomer, CancellationToken cancellationToken = default(CancellationToken));

        Task<IEnumerable<Customer>> GetByNameAsync(string name, CancellationToken cancellationToken = default(CancellationToken));

        Task<Customer> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default(CancellationToken));

        Task<int> GetOrdersCountAsync(Guid customerId, CancellationToken cancellationToken = default(CancellationToken));
    }
}
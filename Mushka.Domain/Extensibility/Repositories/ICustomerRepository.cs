using System.Threading;
using System.Threading.Tasks;
using Mushka.Domain.Entities;

namespace Mushka.Domain.Extensibility.Repositories
{
    public interface ICustomerRepository : IRepositoryBase<Customer>
    {
        Task<Customer> GetByOrderDetails(Customer orderCustomer, CancellationToken cancellationToken = default(CancellationToken));
    }
}
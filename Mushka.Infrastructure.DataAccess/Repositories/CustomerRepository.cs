using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Infrastructure.DataAccess.Database;

namespace Mushka.Infrastructure.DataAccess.Repositories
{
    internal class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(MushkaDbContext context)
            : base(context)
        {
        }

        public Task<Customer> GetByOrderDetails(Customer customer, CancellationToken cancellationToken = default(CancellationToken))
        {
            return dbSet.FirstOrDefaultAsync(cust =>
                cust.FirstName == customer.FirstName &&
                cust.LastName == customer.LastName &&
                cust.Region == customer.Region &&
                cust.City == customer.City, cancellationToken);
        }

        public async Task<IEnumerable<Customer>> GetByName(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await dbSet
                .Where(cus => cus.FirstName.StartsWith(name) || cus.LastName.StartsWith(name))
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
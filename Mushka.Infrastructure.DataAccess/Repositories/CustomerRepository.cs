using System;
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

        public override async Task<Customer> GetByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken)) =>
            await dbSet
                .Where(cust => cust.Id == id)
                .AsNoTracking()
                .Include(cust => cust.Orders)
                .FirstOrDefaultAsync(cancellationToken);

        public Task<Customer> GetByOrderDetailsAsync(Customer customer, CancellationToken cancellationToken = default(CancellationToken))
        {
            return dbSet.FirstOrDefaultAsync(cust =>
                cust.FirstName == customer.FirstName &&
                cust.LastName == customer.LastName &&
                cust.Phone == customer.Phone, cancellationToken);
        }

        public async Task<IEnumerable<Customer>> GetByNameAsync(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await dbSet
                .Where(cust => cust.FirstName.StartsWith(name) || cust.LastName.StartsWith(name))
                .AsNoTracking()
                .Take(10)
                .ToListAsync(cancellationToken);
        }

        public async Task<Customer> GetByPhoneAsync(string phone, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await dbSet
                .Where(cust => cust.Phone == phone)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<int> GetOrdersCountAsync(Guid customerId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await dbSet
                .Where(cust => cust.Id == customerId)
                .AsNoTracking()
                .Select(cust => cust.Orders.Count)
                .SingleAsync(cancellationToken);
        }
    }
}
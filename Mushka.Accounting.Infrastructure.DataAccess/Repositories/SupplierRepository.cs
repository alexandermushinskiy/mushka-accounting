using Mushka.Accounting.Domain.Entities;
using Mushka.Accounting.Domain.Extensibility.Repositories;
using Mushka.Accounting.Infrastructure.DataAccess.Database;

namespace Mushka.Accounting.Infrastructure.DataAccess.Repositories
{
    internal class SupplierRepository : RepositoryBase<Supplier>, ISupplierRepository
    {
        public SupplierRepository(AccountingDbContext context) : base(context)
        {
        }
    }
}
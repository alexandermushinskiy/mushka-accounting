using Mushka.Accounting.Domain.Entities;
using Mushka.Accounting.Domain.Extensibility.Repositories;
using Mushka.Accounting.Infrastructure.DataAccess.Database;

namespace Mushka.Accounting.Infrastructure.DataAccess.Repositories
{
    internal class SizeRepository : RepositoryBase<Size>, ISizeRepository
    {
        public SizeRepository(AccountingDbContext context) : base(context)
        {
        }
    }
}
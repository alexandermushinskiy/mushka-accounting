using Mushka.Accounting.Infrastructure.DataAccess.Database;

namespace Mushka.Accounting.Infrastructure.DataAccess.Repositories
{
    public abstract class Repository
    {
        protected Repository(AccountingDbContext context)
        {
            Context = context;
        }

        protected AccountingDbContext Context { get; }
    }
}
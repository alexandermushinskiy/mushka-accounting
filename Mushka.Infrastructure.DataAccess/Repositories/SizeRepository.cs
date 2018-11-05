using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Infrastructure.DataAccess.Database;

namespace Mushka.Infrastructure.DataAccess.Repositories
{
    internal class SizeRepository : RepositoryBase<Size>, ISizeRepository
    {
        public SizeRepository(MushkaDbContext context) : base(context)
        {
        }
    }
}
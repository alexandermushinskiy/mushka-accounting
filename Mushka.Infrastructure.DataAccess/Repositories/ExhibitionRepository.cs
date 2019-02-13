using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mushka.Domain.Comparers;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Infrastructure.DataAccess.Database;

namespace Mushka.Infrastructure.DataAccess.Repositories
{
    internal class ExhibitionRepository : RepositoryBase<Exhibition>, IExhibitionRepository
    {
        public ExhibitionRepository(MushkaDbContext context) : base(context)
        {
        }
        
        public override async Task<IEnumerable<Exhibition>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken)) =>
            await Context.Exhibitions
                .AsNoTracking()
                .Include(order => order.Products)
                .ToListAsync(cancellationToken);


        public override async Task<Exhibition> GetByIdAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken)) =>
            await Context.Exhibitions
                .Where(order => order.Id == id)
                .AsNoTracking()
                .Include(order => order.Products)
                .ThenInclude(prod => prod.Product.Size)
                .FirstOrDefaultAsync(cancellationToken);

        public override Exhibition Update(Exhibition exhibition)
        {
            var storedExhibition = dbSet
                .AsNoTracking()
                .Include(o => o.Products)
                .Single(o => o.Id == exhibition.Id);

            exhibition.Products
                .ToList()
                .ForEach(op =>
                {
                    Context.Entry(op).State = storedExhibition.Products.Any(sop => sop.ProductId == op.ProductId)
                        ? EntityState.Modified
                        : EntityState.Added;
                });

            storedExhibition.Products
                .Except(exhibition.Products, new ExhibitionProductComparer())
                .ToList()
                .ForEach(sp => Context.Entry(sp).State = EntityState.Deleted);

            Context.Exhibitions.Update(exhibition);

            return exhibition;
        }
    }
}
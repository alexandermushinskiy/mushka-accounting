using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Suppliers.Converters;
using Mushka.Domain.Extensibility.Repositories;

namespace Mushka.Application.Suppliers.Queries.Search
{
    public sealed class SearchSuppliersQueryHandler : IQueryHandler<SearchSuppliersQuery, SearchSuppliersReadModel>
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly ISearchSuppliersConverter _converter;

        public SearchSuppliersQueryHandler(IStorage storage, ISearchSuppliersConverter converter)
        {
            _supplierRepository = storage.GetRepository<ISupplierRepository>();
            _converter = converter;
        }

        public async ValueTask<SearchSuppliersReadModel> Handle(SearchSuppliersQuery query, CancellationToken cancellationToken)
        {
            var suppliers = (await _supplierRepository.GetAllAsync(cancellationToken))
                .OrderBy(supplier => supplier.Name)
                .ToList();

            return _converter.ToReadModel(suppliers);
        }
    }
}

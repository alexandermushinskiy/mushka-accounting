using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Products.Converters;
using Mushka.Domain.Extensibility.Repositories;

namespace Mushka.Application.Products.Queries.Select;

public sealed class SelectProductsQueryHandler : IQueryHandler<SelectProductsQuery, SelectProductsReadModel>
{
    private readonly IProductRepository _productRepository;
    private readonly ISelectProductsConverter _converter;

    public SelectProductsQueryHandler(IStorage storage, ISelectProductsConverter converter)
    {
        _productRepository = storage.GetRepository<IProductRepository>();
        _converter = converter;
    }

    public async ValueTask<SelectProductsReadModel> Handle(SelectProductsQuery query, CancellationToken cancellationToken)
    {
        var products = (query.InStock
                ? await _productRepository.GetAsync(product => product.Quantity > 0, cancellationToken)
                : await _productRepository.GetAllAsync(cancellationToken))
            .OrderBy(product => product.Name)
            .ToList();

        return _converter.ToReadModel(products);
    }
}

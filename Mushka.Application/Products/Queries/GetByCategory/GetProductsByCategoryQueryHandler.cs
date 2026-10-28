using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Products.Converters;
using Mushka.Domain.Extensibility.Repositories;

namespace Mushka.Application.Products.Queries.GetByCategory;

public sealed class GetProductsByCategoryQueryHandler : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryReadModel>
{
    private readonly IProductRepository _productRepository;
    private readonly IGetProductsByCategoryConverter _converter;

    public GetProductsByCategoryQueryHandler(IStorage storage, IGetProductsByCategoryConverter converter)
    {
        _productRepository = storage.GetRepository<IProductRepository>();
        _converter = converter;
    }

    public async ValueTask<GetProductsByCategoryReadModel> Handle(GetProductsByCategoryQuery query, CancellationToken cancellationToken)
    {
        var products = (await _productRepository.GetByCategoryId(query.CategoryId, cancellationToken))
            .OrderBy(product => product.Name)
            .ToList();

        return _converter.ToReadModel(products);
    }
}

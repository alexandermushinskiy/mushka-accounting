using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Products.Converters;
using Mushka.Domain.Extensibility.Repositories;

namespace Mushka.Application.Products.Queries.GetSizes;

public sealed class GetProductSizesQueryHandler : IQueryHandler<GetProductSizesQuery, GetProductSizesReadModel>
{
    private readonly IProductRepository _productRepository;
    private readonly IGetProductSizesConverter _converter;

    public GetProductSizesQueryHandler(IStorage storage, IGetProductSizesConverter converter)
    {
        _productRepository = storage.GetRepository<IProductRepository>();
        _converter = converter;
    }

    public async ValueTask<GetProductSizesReadModel> Handle(GetProductSizesQuery query, CancellationToken cancellationToken)
    {
        var sizes = (await _productRepository.GetSizesAsync(cancellationToken)).ToList();
        return _converter.ToReadModel(sizes);
    }
}

using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Exceptions;
using Mushka.Application.Products.Converters;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;

namespace Mushka.Application.Products.Queries.Describe;

public sealed class DescribeProductQueryHandler : IQueryHandler<DescribeProductQuery, DescribeProductReadModel>
{
    private readonly IProductRepository _productRepository;
    private readonly IDescribeProductConverter _converter;

    public DescribeProductQueryHandler(IStorage storage, IDescribeProductConverter converter)
    {
        _productRepository = storage.GetRepository<IProductRepository>();
        _converter = converter;
    }

    public async ValueTask<DescribeProductReadModel> Handle(DescribeProductQuery query, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(query.Id, cancellationToken);

        if (product == null)
        {
            throw new NotFoundException(ValidationErrors.ProductNotFound);
        }

        return _converter.ToReadModel(product);
    }
}

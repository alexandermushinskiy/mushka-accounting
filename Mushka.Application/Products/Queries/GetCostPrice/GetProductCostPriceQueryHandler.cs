using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Exceptions;
using Mushka.Application.Providers;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;

namespace Mushka.Application.Products.Queries.GetCostPrice;

public sealed class GetProductCostPriceQueryHandler : IQueryHandler<GetProductCostPriceQuery, GetProductCostPriceReadModel>
{
    private readonly IProductRepository _productRepository;
    private readonly ICostPriceProvider _costPriceProvider;

    public GetProductCostPriceQueryHandler(IStorage storage, ICostPriceProvider costPriceProvider)
    {
        _productRepository = storage.GetRepository<IProductRepository>();
        _costPriceProvider = costPriceProvider;
    }

    public async ValueTask<GetProductCostPriceReadModel> Handle(GetProductCostPriceQuery query, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(query.Id, cancellationToken);

        if (product == null)
        {
            throw new NotFoundException(ValidationErrors.ProductNotFound);
        }

        var costPrice = await _costPriceProvider.CalculateAsync(query.Id, query.ProductsCount, cancellationToken);

        return new GetProductCostPriceReadModel
        {
            CostPrice = costPrice
        };
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Orders.Converters;
using Mushka.Application.Providers;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;

namespace Mushka.Application.Orders.Queries.GetDefaultProducts;

public sealed class GetDefaultOrderProductsQueryHandler : IQueryHandler<GetDefaultOrderProductsQuery, GetDefaultOrderProductsReadModel>
{
    private static readonly Guid PostcardId = Guid.Parse("07DF9000-2680-43E7-BA2C-D4F0C48A8CB5");
    private static readonly Guid PackageId = Guid.Parse("A6BBAD88-3820-4972-8AE9-FC931A62A1E7");
    private static readonly Guid[] DefaultProductIds = { PostcardId, PackageId };

    private readonly IProductRepository _productRepository;
    private readonly ICostPriceProvider _costPriceProvider;
    private readonly IGetDefaultOrderProductsConverter _converter;

    public GetDefaultOrderProductsQueryHandler(
        IStorage storage,
        ICostPriceProvider costPriceProvider,
        IGetDefaultOrderProductsConverter converter)
    {
        _productRepository = storage.GetRepository<IProductRepository>();
        _costPriceProvider = costPriceProvider;
        _converter = converter;
    }

    public async ValueTask<GetDefaultOrderProductsReadModel> Handle(
        GetDefaultOrderProductsQuery query,
        CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAsync(
            product => DefaultProductIds.Contains(product.Id) && product.Quantity > 0,
            cancellationToken);

        var orderProducts = new List<OrderProduct>();

        foreach (var product in products)
        {
            orderProducts.Add(new OrderProduct
            {
                ProductId = product.Id,
                Product = product,
                Quantity = 1,
                CostPrice = await _costPriceProvider.CalculateAsync(product.Id, product.Quantity, cancellationToken)
            });
        }

        return _converter.ToReadModel(orderProducts);
    }
}

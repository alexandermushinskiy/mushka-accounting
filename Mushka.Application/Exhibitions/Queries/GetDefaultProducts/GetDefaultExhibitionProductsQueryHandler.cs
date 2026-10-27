using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Exhibitions.Converters;
using Mushka.Application.Providers;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;

namespace Mushka.Application.Exhibitions.Queries.GetDefaultProducts;

public sealed class GetDefaultExhibitionProductsQueryHandler :
    IQueryHandler<GetDefaultExhibitionProductsQuery, GetDefaultExhibitionProductsReadModel>
{
    private static readonly Guid PostcardId = Guid.Parse("07DF9000-2680-43E7-BA2C-D4F0C48A8CB5");
    private static readonly Guid PackageId = Guid.Parse("A6BBAD88-3820-4972-8AE9-FC931A62A1E7");
    private static readonly Guid[] DefaultProductIds = { PostcardId, PackageId };

    private readonly IProductRepository _productRepository;
    private readonly ICostPriceProvider _costPriceProvider;
    private readonly IGetDefaultExhibitionProductsConverter _converter;

    public GetDefaultExhibitionProductsQueryHandler(
        IStorage storage,
        ICostPriceProvider costPriceProvider,
        IGetDefaultExhibitionProductsConverter converter)
    {
        _productRepository = storage.GetRepository<IProductRepository>();
        _costPriceProvider = costPriceProvider;
        _converter = converter;
    }

    public async ValueTask<GetDefaultExhibitionProductsReadModel> Handle(
        GetDefaultExhibitionProductsQuery query,
        CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAsync(
            product => DefaultProductIds.Contains(product.Id) && product.Quantity > 0,
            cancellationToken);

        var exhibitionProducts = new List<ExhibitionProduct>();

        foreach (var product in products)
        {
            exhibitionProducts.Add(new ExhibitionProduct
            {
                ProductId = product.Id,
                Product = product,
                Quantity = 1,
                CostPrice = await _costPriceProvider.CalculateAsync(product.Id, product.Quantity, cancellationToken)
            });
        }

        return _converter.ToReadModel(exhibitionProducts);
    }
}

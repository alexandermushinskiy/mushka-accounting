using Mushka.Application.Products.Commands;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;

namespace Mushka.Application.Products.Converters;

public sealed class CreateProductConverter : ICreateProductConverter
{
    private readonly IGuidProvider _guidProvider;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateProductConverter(IGuidProvider guidProvider, IDateTimeProvider dateTimeProvider)
    {
        _guidProvider = guidProvider;
        _dateTimeProvider = dateTimeProvider;
    }

    public Product ToProduct(CreateProductCommand command) =>
        new Product
        {
            Id = _guidProvider.NewGuid(),
            CategoryId = command.Product.CategoryId,
            CreatedOn = _dateTimeProvider.GetNow(),
            Name = command.Product.Name,
            VendorCode = command.Product.VendorCode,
            RecommendedPrice = command.Product.RecommendedPrice,
            SizeId = command.Product.SizeId,
            IsArchival = command.Product.IsArchival
        };
}

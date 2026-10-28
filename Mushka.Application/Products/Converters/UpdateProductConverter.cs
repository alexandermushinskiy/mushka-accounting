using Mushka.Application.Products.Commands;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;

namespace Mushka.Application.Products.Converters;

public sealed class UpdateProductConverter : IUpdateProductConverter
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateProductConverter(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public Product ToProduct(UpdateProductCommand command) =>
        new Product
        {
            Id = command.Id,
            CategoryId = command.Product.CategoryId,
            CreatedOn = _dateTimeProvider.GetNow(),
            Name = command.Product.Name,
            VendorCode = command.Product.VendorCode,
            RecommendedPrice = command.Product.RecommendedPrice,
            SizeId = command.Product.SizeId,
            IsArchival = command.Product.IsArchival
        };
}

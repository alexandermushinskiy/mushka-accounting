using Mushka.Application.Products.Queries.Describe;
using Mushka.Domain.Entities;

namespace Mushka.Application.Products.Converters;

public sealed class DescribeProductConverter : IDescribeProductConverter
{
    public DescribeProductReadModel ToReadModel(Product product) =>
        new DescribeProductReadModel
        {
            Id = product.Id,
            Name = product.Name,
            VendorCode = product.VendorCode,
            RecommendedPrice = product.RecommendedPrice,
            CreatedOn = product.CreatedOn,
            CategoryId = product.CategoryId,
            Category = ToCategory(product.Category),
            Size = ToSize(product.Size),
            IsArchival = product.IsArchival
        };

    private static DescribeProductCategoryReadModel ToCategory(Category category) =>
        category == null
            ? null
            : new DescribeProductCategoryReadModel
            {
                Id = category.Id,
                Name = category.Name
            };

    private static DescribeProductSizeReadModel ToSize(Size size) =>
        size == null
            ? null
            : new DescribeProductSizeReadModel
            {
                Id = size.Id,
                Name = size.Name
            };
}

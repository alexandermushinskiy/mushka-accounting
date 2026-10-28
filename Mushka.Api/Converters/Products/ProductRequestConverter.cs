using System;
using Mushka.Api.ClientModels.Product;
using Mushka.Application.Products.Commands;

namespace Mushka.Api.Converters.Products;

public sealed class ProductRequestConverter : IProductRequestConverter
{
    public CreateProductCommand ToCreateCommand(CreateProductRequestModel request) =>
        new CreateProductCommand(ToProduct(request.Name, request.VendorCode, request.RecommendedPrice, request.CategoryId, request.SizeId, request.IsArchival));

    public UpdateProductCommand ToUpdateCommand(Guid id, UpdateProductRequestModel request) =>
        new UpdateProductCommand(id, ToProduct(request.Name, request.VendorCode, request.RecommendedPrice, request.CategoryId, request.SizeId, request.IsArchival));

    private static ProductData ToProduct(
        string name,
        string vendorCode,
        decimal? recommendedPrice,
        Guid categoryId,
        Guid? sizeId,
        bool isArchival) =>
        new ProductData(name, vendorCode, recommendedPrice, categoryId, sizeId, isArchival);
}

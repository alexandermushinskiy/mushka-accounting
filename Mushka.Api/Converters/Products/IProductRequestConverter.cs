using System;
using Mushka.Api.ClientModels.Product;
using Mushka.Application.Products.Commands;

namespace Mushka.Api.Converters.Products;

public interface IProductRequestConverter
{
    CreateProductCommand ToCreateCommand(CreateProductRequestModel request);

    UpdateProductCommand ToUpdateCommand(Guid id, UpdateProductRequestModel request);
}

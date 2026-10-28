using Mushka.Application.Products.Commands;
using Mushka.Domain.Entities;

namespace Mushka.Application.Products.Converters;

public interface ICreateProductConverter
{
    Product ToProduct(CreateProductCommand command);
}

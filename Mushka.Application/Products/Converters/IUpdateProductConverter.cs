using Mushka.Application.Products.Commands;
using Mushka.Domain.Entities;

namespace Mushka.Application.Products.Converters;

public interface IUpdateProductConverter
{
    Product ToProduct(UpdateProductCommand command);
}

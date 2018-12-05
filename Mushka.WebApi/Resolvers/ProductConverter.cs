using System.Linq;
using AutoMapper;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Product;

namespace Mushka.WebApi.Resolvers
{
    public class ProductConverter : ITypeConverter<Product, ProductModel>
    {
        public ProductModel Convert(Product source, ProductModel destination, ResolutionContext context) =>
            new ProductModel
                {
                    Id = source.Id,
                    Name = source.Name,
                    Code = source.Code,
                    CreatedOn = source.CreatedOn,
                    Sizes = source.Sizes.Select(CreateProductSizeModel).ToArray()
                };

        private static ProductSizeModel CreateProductSizeModel(ProductSize productSize) =>
            new ProductSizeModel
            {
                Id = productSize.SizeId,
                Name = productSize.Size.Name,
                Quantity = productSize.Quantity
            };
    }
}
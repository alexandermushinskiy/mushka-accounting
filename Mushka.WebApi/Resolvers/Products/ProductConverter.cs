using AutoMapper;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Product;

namespace Mushka.WebApi.Resolvers
{
    public class ProductConverter : ITypeConverter<Product, ProductModel>
    {
        public ProductModel Convert(Product source, ProductModel destination, ResolutionContext context)
        {
            return new ProductModel
            {
                Id = source.Id,
                Name = source.Name,
                VendorCode = source.VendorCode,
                RecommendedPrice = source.RecommendedPrice,
                CreatedOn = source.CreatedOn,
                IsArchival = source.IsArchival
            };
        }
    }
}
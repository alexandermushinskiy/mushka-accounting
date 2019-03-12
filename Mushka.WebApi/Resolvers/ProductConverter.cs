using AutoMapper;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Product;

namespace Mushka.WebApi.Resolvers
{
    public class ProductConverter : ITypeConverter<Product, ProductModel>
    {
        public ProductModel Convert(Product source, ProductModel destination, ResolutionContext context)
        {
            var size = source.Size == null ? null : new SizeModel { Id = source.Size.Id, Name = source.Size.Name };

            return new ProductModel
            {
                Id = source.Id,
                Name = source.Name,
                VendorCode = source.VendorCode,
                RecommendedPrice = source.RecommendedPrice,
                CreatedOn = source.CreatedOn,
                CategoryId = source.CategoryId,
                Category = ConvertToCategoryModel(source.Category),
                Size = size,
                IsAdditional = source.IsAdditional,
                IsArchival = source.IsArchival
            };
        }

        private static CategoryModel ConvertToCategoryModel(Category category) =>
            category == null ? null : new CategoryModel { Id = category.Id, Name = category.Name };
    }
}
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Product;
using Mushka.WebApi.ClientModels.Product.GetById;

namespace Mushka.WebApi.Resolvers.Products
{
    public class ProductResponseConverter : ITypeConverter<OperationResult<Product>, ProductResponseModel>
    {
        public ProductResponseModel Convert(
            OperationResult<Product> source,
            ProductResponseModel destination,
            ResolutionContext context)
        {
            return new ProductResponseModel
            {
                Category = ConvertToCategoryModel(source.Data),
                Product = Mapper.Map<Product, ProductModel>(source.Data),
                Size = ConvertToSizeModel(source.Data)
            };
        }

        private CategoryModel ConvertToCategoryModel(Product product)
            => new CategoryModel { Id = product.Category.Id, Name = product.Category.Name };

        private SizeModel ConvertToSizeModel(Product product)
            => product.Size == null ? null : new SizeModel { Id = product.Size.Id, Name = product.Size.Name };
    }
}
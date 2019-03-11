using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Product;

namespace Mushka.WebApi.Resolvers
{
    public class ProductResponseResolver :
        IValueResolver<ValidationResponse<Product>, ProductResponseModel, ProductModel>
    {
        public ProductModel Resolve(
            ValidationResponse<Product> source,
            ProductResponseModel destination,
            ProductModel destMember,
            ResolutionContext context) => source.Result == null ? null : Mapper.Map<Product, ProductModel>(source.Result);
    }
}
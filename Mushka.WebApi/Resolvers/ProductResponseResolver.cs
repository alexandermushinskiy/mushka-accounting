using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Product;

namespace Mushka.WebApi.Resolvers
{
    public class ProductResponseResolver :
        IValueResolver<OperationResult<Product>, ProductResponseModel, ProductModel>
    {
        public ProductModel Resolve(
            OperationResult<Product> source,
            ProductResponseModel destination,
            ProductModel destMember,
            ResolutionContext context) => source.Data == null ? null : Mapper.Map<Product, ProductModel>(source.Data);
    }
}
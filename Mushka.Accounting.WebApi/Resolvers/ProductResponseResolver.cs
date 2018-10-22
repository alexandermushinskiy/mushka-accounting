using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Accounting.Core.Validation;
using Mushka.Accounting.Domain.Entities;
using Mushka.Accounting.WebApi.ClientModels.Product;

namespace Mushka.Accounting.WebApi.Resolvers
{
    public class ProductResponseResolver :
        IValueResolver<ValidationResponse<Product>, ProductResponseModel, ProductModel>,
        IValueResolver<ValidationResponse<IEnumerable<Product>>, ProductsResponseModel, IEnumerable<ProductModel>>
    {
        public ProductModel Resolve(
            ValidationResponse<Product> source,
            ProductResponseModel destination,
            ProductModel destMember,
            ResolutionContext context) => source.Result == null ? null : Mapper.Map<Product, ProductModel>(source.Result);

        public IEnumerable<ProductModel> Resolve(
            ValidationResponse<IEnumerable<Product>> source,
            ProductsResponseModel destination,
            IEnumerable<ProductModel> destMember,
            ResolutionContext context) => source.Result?.Select(Mapper.Map<Product, ProductModel>);
    }
}
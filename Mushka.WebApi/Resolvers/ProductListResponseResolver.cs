using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Product;

namespace Mushka.WebApi.Resolvers
{
    public class ProductListResponseResolver :
        IValueResolver<ValidationResponse<IEnumerable<Product>>, ProductListResponseModel, IEnumerable<ProductListModel>>
    {
        public IEnumerable<ProductListModel> Resolve(
            ValidationResponse<IEnumerable<Product>> source,
            ProductListResponseModel destination,
            IEnumerable<ProductListModel> destMember,
            ResolutionContext context)
        {
            return source.Result?.Select(Mapper.Map<Product, ProductListModel>);
        }
    }
}
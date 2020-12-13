using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Product;

namespace Mushka.WebApi.Resolvers
{
    public class ProductListResponseResolver :
        IValueResolver<OperationResult<IEnumerable<Product>>, ProductListResponseModel, IEnumerable<ProductListModel>>
    {
        public IEnumerable<ProductListModel> Resolve(
            OperationResult<IEnumerable<Product>> source,
            ProductListResponseModel destination,
            IEnumerable<ProductListModel> destMember,
            ResolutionContext context) => source.Data?.Select(Mapper.Map<Product, ProductListModel>);
    }
}
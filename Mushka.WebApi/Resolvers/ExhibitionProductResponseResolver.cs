using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Exhibition;

namespace Mushka.WebApi.Resolvers
{
    public class ExhibitionProductResponseResolver : IValueResolver<OperationResult<IEnumerable<ExhibitionProduct>>, ExhibitionProductsResponseModel, IEnumerable<ExhibitionProductModel>>
    {
        public IEnumerable<ExhibitionProductModel> Resolve(
            OperationResult<IEnumerable<ExhibitionProduct>> source,
            ExhibitionProductsResponseModel destination,
            IEnumerable<ExhibitionProductModel> destMember,
            ResolutionContext context) => source.Data?.Select(Mapper.Map<ExhibitionProduct, ExhibitionProductModel>);
    }
}
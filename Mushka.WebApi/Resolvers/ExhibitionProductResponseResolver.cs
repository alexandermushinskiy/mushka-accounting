using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Exhibition;

namespace Mushka.WebApi.Resolvers
{
    public class ExhibitionProductResponseResolver : IValueResolver<ValidationResponse<IEnumerable<ExhibitionProduct>>, ExhibitionProductsResponseModel, IEnumerable<ExhibitionProductModel>>
    {
        public IEnumerable<ExhibitionProductModel> Resolve(
            ValidationResponse<IEnumerable<ExhibitionProduct>> source,
            ExhibitionProductsResponseModel destination,
            IEnumerable<ExhibitionProductModel> destMember,
            ResolutionContext context) => source.Result?.Select(Mapper.Map<ExhibitionProduct, ExhibitionProductModel>);
    }
}
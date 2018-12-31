using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Supply;

namespace Mushka.WebApi.Resolvers
{
    public class SupplyResponseResolver :
        IValueResolver<ValidationResponse<Supply>, SupplyResponseModel, SupplyModel>,
        IValueResolver<ValidationResponse<IEnumerable<Supply>>, SuppliesResponseModel, IEnumerable<SupplyModel>>
    {
        public SupplyModel Resolve(
            ValidationResponse<Supply> source,
            SupplyResponseModel destination,
            SupplyModel destMember,
            ResolutionContext context) => source.Result == null ? null : Mapper.Map<Supply, SupplyModel>(source.Result);

        public IEnumerable<SupplyModel> Resolve(
            ValidationResponse<IEnumerable<Supply>> source,
            SuppliesResponseModel destination,
            IEnumerable<SupplyModel> destMember,
            ResolutionContext context) => source.Result?.Select(Mapper.Map<Supply, SupplyModel>);
    }
}
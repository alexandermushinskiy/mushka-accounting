using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Supply;

namespace Mushka.WebApi.Resolvers
{
    public class SupplyResponseResolver : IValueResolver<OperationResult<Supply>, SupplyResponseModel, SupplyModel>
    {
        public SupplyModel Resolve(
            OperationResult<Supply> source,
            SupplyResponseModel destination,
            SupplyModel destMember,
            ResolutionContext context) => source.Data == null ? null : Mapper.Map<Supply, SupplyModel>(source.Data);
    }
}
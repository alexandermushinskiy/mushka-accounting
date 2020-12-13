using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Exhibition;

namespace Mushka.WebApi.Resolvers
{
    public class ExhibitionResponseResolver :
        IValueResolver<OperationResult<IEnumerable<Exhibition>>, ExhibitionsResponseModel, IEnumerable<ExhibitionModel>>,
        IValueResolver<OperationResult<Exhibition>, ExhibitionResponseModel, ExhibitionModel>
    {
        public IEnumerable<ExhibitionModel> Resolve(
            OperationResult<IEnumerable<Exhibition>> source,
            ExhibitionsResponseModel destination,
            IEnumerable<ExhibitionModel> destMember,
            ResolutionContext context) => source.Data?.Select(Mapper.Map<Exhibition, ExhibitionModel>);

        public ExhibitionModel Resolve(
            OperationResult<Exhibition> source,
            ExhibitionResponseModel destination,
            ExhibitionModel destMember,
            ResolutionContext context) => source.Data == null ? null : Mapper.Map<Exhibition, ExhibitionModel>(source.Data);
    }
}
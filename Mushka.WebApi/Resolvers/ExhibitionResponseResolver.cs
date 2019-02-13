using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Exhibition;

namespace Mushka.WebApi.Resolvers
{
    public class ExhibitionResponseResolver :
        IValueResolver<ValidationResponse<Exhibition>, ExhibitionResponseModel, ExhibitionModel>,
        IValueResolver<ValidationResponse<IEnumerable<Exhibition>>, ExhibitionsResponseModel, IEnumerable<ExhibitionModel>>
    {
        public ExhibitionModel Resolve(
            ValidationResponse<Exhibition> source,
            ExhibitionResponseModel destination,
            ExhibitionModel destMember,
            ResolutionContext context) => source.Result == null ? null : Mapper.Map<Exhibition, ExhibitionModel>(source.Result);

        public IEnumerable<ExhibitionModel> Resolve(
            ValidationResponse<IEnumerable<Exhibition>> source,
            ExhibitionsResponseModel destination,
            IEnumerable<ExhibitionModel> destMember,
            ResolutionContext context) => source.Result?.Select(Mapper.Map<Exhibition, ExhibitionModel>);
    }
}
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Product;

namespace Mushka.WebApi.Resolvers
{
    public class SizeResponseResolver : IValueResolver<ValidationResponse<IEnumerable<Size>>, SizesResponseModel, IEnumerable<SizeModel>>
    {
        public IEnumerable<SizeModel> Resolve(
            ValidationResponse<IEnumerable<Size>> source,
            SizesResponseModel destination,
            IEnumerable<SizeModel> destMember,
            ResolutionContext context)
        {
            return source.Result.Select(sz => new SizeModel { Id = sz.Id, Name = sz.Name });
        }
    }
}
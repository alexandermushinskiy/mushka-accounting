using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Exhibition;

namespace Mushka.WebApi.Resolvers
{
    public class ExhibitionsListResponseResolver : IValueResolver<ValidationResponse<IEnumerable<Exhibition>>, ExhibitionsListResponseModel, IEnumerable<ExhibitionsListModel>>
    {
        public IEnumerable<ExhibitionsListModel> Resolve(
            ValidationResponse<IEnumerable<Exhibition>> source,
            ExhibitionsListResponseModel destination,
            IEnumerable<ExhibitionsListModel> destMember,
            ResolutionContext context)
        {
            var t1 = source.Result?.Select(exhibition => new ExhibitionsListModel
            {
                Id = exhibition.Id,
                Name = exhibition.Name,
                FromDate = exhibition.FromDate,
                ToDate = exhibition.ToDate,
                City = exhibition.City,
                ParticipationCost = exhibition.ParticipationCost,
                ProductsCount = exhibition.Products.Sum(p => p.Quantity),
            });
            return t1;
        }
    }
}
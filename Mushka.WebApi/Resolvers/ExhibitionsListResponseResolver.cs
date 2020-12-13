using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Exhibition;

namespace Mushka.WebApi.Resolvers
{
    public class ExhibitionsListResponseResolver : IValueResolver<OperationResult<IEnumerable<Exhibition>>, ExhibitionsListResponseModel, IEnumerable<ExhibitionsListModel>>
    {
        public IEnumerable<ExhibitionsListModel> Resolve(
            OperationResult<IEnumerable<Exhibition>> source,
            ExhibitionsListResponseModel destination,
            IEnumerable<ExhibitionsListModel> destMember,
            ResolutionContext context)
        {
            return source.Data?.Select(exhibition => new ExhibitionsListModel
            {
                Id = exhibition.Id,
                Name = exhibition.Name,
                FromDate = exhibition.FromDate,
                ToDate = exhibition.ToDate,
                City = exhibition.City,
                ParticipationCost = exhibition.ParticipationCost,
                ProductsCount = exhibition.Products.Sum(p => p.Quantity),
            });
        }
    }
}
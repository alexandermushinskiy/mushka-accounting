using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Exhibition.Search;

namespace Mushka.WebApi.Resolvers.Exhibitions
{
    public class SearchExhibitionsResponseConverter :
        ITypeConverter<OperationResult<IEnumerable<Exhibition>>, SearchExhibitionsResponseModel>
    {
        public SearchExhibitionsResponseModel Convert(
            OperationResult<IEnumerable<Exhibition>> source,
            SearchExhibitionsResponseModel destination,
            ResolutionContext context)
        {
            return new SearchExhibitionsResponseModel
            {
                Total = source.Data.Count(),
                Items = source.Data.Select(ConvertToSearchExhibitionModel)
            };
        }

        private SearchExhibitionModel ConvertToSearchExhibitionModel(Exhibition exhibition)
        {
            return new SearchExhibitionModel
            {
                Id = exhibition.Id,
                Name = exhibition.Name,
                FromDate = exhibition.FromDate,
                ToDate = exhibition.ToDate,
                City = exhibition.City,
                ParticipationCost = exhibition.ParticipationCost,
                ProductsCount = exhibition.Products.Sum(p => p.Quantity),
            };
        }
    }
}
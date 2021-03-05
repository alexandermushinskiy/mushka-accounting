using AutoMapper;
using Mushka.Domain.Models;
using Mushka.WebApi.ClientModels.Supply.Search;

namespace Mushka.WebApi.Resolvers.Supplies
{
    public class SearchSuppliesRequestConverter : ITypeConverter<SearchSuppliesRequestModel, SearchSuppliesFilter>
    {
        public SearchSuppliesFilter Convert(
            SearchSuppliesRequestModel source,
            SearchSuppliesFilter destination,
            ResolutionContext context)
        {
            return new SearchSuppliesFilter
            {
                SearchKey = source.SearchKey,
                ProductId = source.ProductId
            };
        }
    }
}
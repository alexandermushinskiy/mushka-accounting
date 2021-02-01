using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Category.Search;

namespace Mushka.WebApi.Resolvers.Categories
{
    public class SearchCategoriesResponseConverter : ITypeConverter<OperationResult<IEnumerable<Category>>, SearchCategoriesResponseModel>
    {
        public SearchCategoriesResponseModel Convert(
            OperationResult<IEnumerable<Category>> source,
            SearchCategoriesResponseModel destination,
            ResolutionContext context)
        {
            return new SearchCategoriesResponseModel
            {
                Total = source.Data?.Count() ?? 0,
                Items = source.Data?.Select(ConvertToCategorySummaryModel) ?? Enumerable.Empty<CategorySummaryModel>()
            };
        }

        private static CategorySummaryModel ConvertToCategorySummaryModel(Category category)
        {
            return new CategorySummaryModel
            {
                Id = category.Id,
                Name = category.Name,
                IsSizeRequired = category.IsSizeRequired,
                IsAdditional = category.IsAdditional
            };
        }
    }
}
using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Category.Search
{
    public class SearchCategoriesResponseModel
    {
        public int Total { get; set; }
        public IEnumerable<CategorySummaryModel> Items { get; set; }
    }
}
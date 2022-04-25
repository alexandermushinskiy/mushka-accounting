using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Product
{
    public class SearchProductsResponseModel
    {
        public int Total { get; set; }
        public IEnumerable<ProductSummaryModel> Items { get; set; }
    }
}
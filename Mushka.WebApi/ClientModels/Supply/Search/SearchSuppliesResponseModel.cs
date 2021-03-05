using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Supply.Search
{
    public class SearchSuppliesResponseModel
    {
        public int Total { get; set; }
        public IEnumerable<SearchSupplyResponseModel> Items { get; set; }
    }
}
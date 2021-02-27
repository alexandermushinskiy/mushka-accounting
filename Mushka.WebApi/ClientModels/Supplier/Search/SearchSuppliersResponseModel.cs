using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Supplier.Search
{
    public class SearchSuppliersResponseModel
    {
        public int Total { get; set; }
        public IEnumerable<SearchSupplierResponseModel> Items { get; set; }
    }
}
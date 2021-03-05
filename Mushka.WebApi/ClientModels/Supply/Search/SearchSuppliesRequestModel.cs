using System;

namespace Mushka.WebApi.ClientModels.Supply.Search
{
    public class SearchSuppliesRequestModel
    {
        public string SearchKey { get; set; }
        public Guid? ProductId { get; set; }
    }
}
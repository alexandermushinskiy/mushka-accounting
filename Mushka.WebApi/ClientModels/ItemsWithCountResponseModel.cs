using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels
{
    public class ItemsWithCountResponseModel<TData>
    {
        public IEnumerable<TData> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
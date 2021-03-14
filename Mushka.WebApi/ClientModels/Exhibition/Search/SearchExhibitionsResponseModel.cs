using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Exhibition.Search
{
    public class SearchExhibitionsResponseModel
    {
        public int Total { get; set; }
        public IEnumerable<SearchExhibitionModel> Items { get; set; }
    }
}
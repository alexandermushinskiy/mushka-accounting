using System;

namespace Mushka.Domain.Models
{
    public class SearchOrdersFilter
    {
        public string Criteria { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public string SortKey { get; set; }
        public string SortOrder { get; set; }
    }
}
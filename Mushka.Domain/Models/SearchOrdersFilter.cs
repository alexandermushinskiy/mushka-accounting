namespace Mushka.Domain.Models
{
    public class SearchOrdersFilter
    {
        public string CustomerName { get; set; }
        public DateRange OrderDate { get; set; }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public string SortKey { get; set; }
        public string SortOrder { get; set; }
    }
}
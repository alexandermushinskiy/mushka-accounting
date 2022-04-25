namespace Mushka.Domain.Models
{
    public class SearchOrdersFilter
    {
        public string SearchKey { get; set; }
        public string Region { get; set; }
        public DateRange OrderDate { get; set; }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public string SortKey { get; set; }
        public bool IsAsc { get; set; }
    }
}
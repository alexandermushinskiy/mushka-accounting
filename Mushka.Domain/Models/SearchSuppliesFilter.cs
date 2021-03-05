using System;

namespace Mushka.Domain.Models
{
    public class SearchSuppliesFilter
    {
        public string SearchKey { get; set; }
        public Guid? ProductId { get; set; }
    }
}
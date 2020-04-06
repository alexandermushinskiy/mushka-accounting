using System;

namespace Mushka.Domain.Models
{
    public class SuppliesFiltersModel
    {
        public string SearchKey { get; set; }
        public Guid? ProductId { get; set; }
    }
}
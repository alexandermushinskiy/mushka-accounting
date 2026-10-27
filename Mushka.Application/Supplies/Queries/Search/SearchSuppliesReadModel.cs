using System;
using System.Collections.Generic;

namespace Mushka.Application.Supplies.Queries.Search
{
    public sealed class SearchSuppliesReadModel
    {
        public int Total { get; set; }

        public IEnumerable<SearchSupplyReadModel> Items { get; set; }
    }

    public sealed class SearchSupplyReadModel
    {
        public Guid Id { get; set; }

        public DateTime ReceivedDate { get; set; }

        public string Description { get; set; }

        public string SupplierName { get; set; }

        public int ProductsAmount { get; set; }

        public decimal Cost { get; set; }

        public decimal TotalCost { get; set; }
    }
}

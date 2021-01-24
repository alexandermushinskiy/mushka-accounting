using System;

namespace Mushka.WebApi.ClientModels.Order.Search
{
    public class OrderSummaryModel
    {
        public Guid Id { get; set; }

        public DateTime OrderDate { get; set; }

        public string OrderNumber { get; set; }

        public decimal Cost { get; set; }

        public string Address { get; set; }

        public string CustomerName { get; set; }

        public int ProductsCount { get; set; }

        public bool IsWholesale { get; set; }
    }
}
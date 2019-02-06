using System;

namespace Mushka.WebApi.ClientModels.Order
{
    public class OrderListModel
    {
        public Guid Id { get; set; }

        public DateTime OrderDate { get; set; }

        public string Number { get; set; }

        public decimal Cost { get; set; }

        public string Address { get; set; }

        public string CustomerName { get; set; }

        public int ProductsCount { get; set; }
    }
}
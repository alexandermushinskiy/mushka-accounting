using System;
using Mushka.Domain.Entities;

namespace Mushka.WebApi.ClientModels.Order.GetById
{
    public class OrderModel
    {
        public Guid Id { get; set; }

        public DateTime OrderDate { get; set; }

        public string Number { get; set; }

        public decimal Cost { get; set; }

        public PaymentMethod CostMethod { get; set; }

        public int Discount { get; set; }

        public decimal Profit { get; set; }

        public string Region { get; set; }

        public string City { get; set; }

        public bool IsWholesale { get; set; }

        public string Notes { get; set; }
    }
}
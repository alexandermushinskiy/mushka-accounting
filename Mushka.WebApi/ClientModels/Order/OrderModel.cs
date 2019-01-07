using System;
using System.Collections.Generic;
using Mushka.Domain.Entities;

namespace Mushka.WebApi.ClientModels.Order
{
    public class OrderModel
    {
        public Guid Id { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal Cost { get; set; }

        public PaymentMethod CostMethod { get; set; }

        public IEnumerable<OrderProductModel> Products { get; set; }
    }
}
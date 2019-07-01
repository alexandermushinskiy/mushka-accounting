using System;
using System.Collections.Generic;
using Mushka.Domain.Entities;

namespace Mushka.WebApi.ClientModels.Order
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
        
        //public string FirstName { get; set; }
        
        //public string LastName { get; set; }

        //public string Phone { get; set; }

        //public string Email { get; set; }

        public bool IsWholesale { get; set; }

        public string Notes { get; set; }

        public OrderCustomerModel Customer { get; set; }

        public IEnumerable<OrderProductModel> Products { get; set; }
    }
}
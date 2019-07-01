using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mushka.Domain.Entities;
using Mushka.WebApi.Filters;

namespace Mushka.WebApi.ClientModels.Order
{
    public class OrderRequestModel
    {
        [Required]
        public string Number { get; set; }

        [RequireNonDefault]
        public DateTime OrderDate { get; set; }

        [RequireNonDefault]
        public decimal Cost { get; set; }

        [RequireNonDefault]
        public PaymentMethod CostMethod { get; set; }

        public int? Discount { get; set; }

        [RequireNonDefault]
        public decimal Profit { get; set; }

        [Required]
        public string Region { get; set; }

        [Required]
        public string City { get; set; }

        //[Required]
        //public string FirstName { get; set; }

        //[Required]
        //public string LastName { get; set; }

        //public string Phone { get; set; }

        //public string Email { get; set; }

        public bool IsWholesale { get; set; }

        public string Notes { get; set; }

        [Required]
        public OrderCustomerRequestModel Customer { get; set; }

        [Required]
        [MinLength(1)]
        public IEnumerable<OrderProductRequestModel> Products { get; set; }
    }
}
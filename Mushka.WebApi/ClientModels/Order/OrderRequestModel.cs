using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mushka.Domain.Entities;

namespace Mushka.WebApi.ClientModels.Order
{
    public class OrderRequestModel
    {
        public string Number { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public decimal Cost { get; set; }

        [Required]
        public PaymentMethod CostMethod { get; set; }

        [Required]
        public decimal Profit { get; set; }

        [Required]
        public string Region { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string Notes { get; set; }

        [Required]
        [MinLength(1)]
        public IEnumerable<OrderProductRequestModel> Products { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mushka.Domain.Entities;

namespace Mushka.WebApi.ClientModels.Order
{
    public class OrderRequestModel
    {
        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        [MinLength(1)]
        public IEnumerable<OrderProductRequestModel> Products { get; set; }
    }
}
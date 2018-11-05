using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mushka.Domain.Entities;

namespace Mushka.WebApi.ClientModels.Delivery
{
    public class DeliveryRequestModel
    {
        [Required]
        public DateTime RequestDate { get; set; }

        [Required]
        public DateTime DeliveryDate { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [Required]
        public decimal Cost { get; set; }

        public decimal TransferFee { get; set; }

        [Required]
        [MinLength(1)]
        public IEnumerable<DeliveryProductRequestModel> Products { get; set; }
    }
}
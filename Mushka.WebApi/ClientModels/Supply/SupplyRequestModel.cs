using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mushka.Domain.Entities;

namespace Mushka.WebApi.ClientModels.Supply
{
    public class SupplyRequestModel
    {
        public Guid SupplierId { get; set; }

        [Required]
        public DateTime RequestDate { get; set; }

        [Required]
        public DateTime ReceivedDate { get; set; }

        public decimal? Prepayment { get; set; }

        public PaymentMethod? PrepaymentMethod { get; set; }

        public decimal Cost { get; set; }

        public PaymentMethod CostMethod { get; set; }

        public decimal? DeliveryCost { get; set; }

        public PaymentMethod? DeliveryCostMethod { get; set; }

        public decimal? BankFee { get; set; }

        public decimal TotalCost { get; set; }

        public string Notes { get; set; }

        [Required]
        [MinLength(1)]
        public IEnumerable<SupplyProductRequestModel> Products { get; set; }
    }
}
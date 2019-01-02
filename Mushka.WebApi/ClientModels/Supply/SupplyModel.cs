using System;
using System.Collections.Generic;
using Mushka.Domain.Entities;

namespace Mushka.WebApi.ClientModels.Supply
{
    public class SupplyModel
    {
        public Guid Id { get; set; }

        public DateTime RequestDate { get; set; }

        public DateTime ReceivedDate { get; set; }
        
        public decimal Cost { get; set; }

        public PaymentMethod CostMethod { get; set; }

        public decimal? Prepayment { get; set; }

        public PaymentMethod? PrepaymentMethod { get; set; }

        public decimal? DeliveryCost { get; set; }

        public PaymentMethod? DeliveryCostMethod { get; set; }

        public decimal? BankFee { get; set; }
        
        public decimal TotalCost { get; set; }

        public Guid SupplierId { get; set; }

        public string SupplierName { get; set; }

        public int ProductsAmount { get; set; }

        public string Notes { get; set; }

        public IEnumerable<SupplyProductModel> Products { get; set; }
    }
}
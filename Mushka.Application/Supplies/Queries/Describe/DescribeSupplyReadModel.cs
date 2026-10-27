using System;
using System.Collections.Generic;
using Mushka.Domain.Entities;

namespace Mushka.Application.Supplies.Queries.Describe
{
    public sealed class DescribeSupplyReadModel
    {
        public DescribeSupplyDetailsReadModel Supply { get; set; }

        public IEnumerable<DescribeSupplyProductReadModel> Products { get; set; }
    }

    public sealed class DescribeSupplyDetailsReadModel
    {
        public Guid Id { get; set; }

        public DateTime RequestDate { get; set; }

        public DateTime ReceivedDate { get; set; }

        public decimal Cost { get; set; }

        public string Description { get; set; }

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
    }

    public sealed class DescribeSupplyProductReadModel
    {
        public DescribeSupplyProductDetailsReadModel Product { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal CostPrice { get; set; }
    }

    public sealed class DescribeSupplyProductDetailsReadModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string VendorCode { get; set; }

        public string SizeName { get; set; }
    }
}

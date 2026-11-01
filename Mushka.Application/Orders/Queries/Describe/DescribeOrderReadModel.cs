using System;
using System.Collections.Generic;
using Mushka.Domain.Entities;

namespace Mushka.Application.Orders.Queries.Describe;

public sealed class DescribeOrderReadModel
{
    public DescribeOrderDetailsReadModel Order { get; set; }

    public DescribeOrderCustomerReadModel Customer { get; set; }

    public IEnumerable<DescribeOrderProductReadModel> Products { get; set; }
}

public sealed class DescribeOrderDetailsReadModel
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

public sealed class DescribeOrderCustomerReadModel
{
    public Guid Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Phone { get; set; }

    public string Email { get; set; }
}

public sealed class DescribeOrderProductReadModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string VendorCode { get; set; }

    public string SizeName { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal CostPrice { get; set; }
}

using System;
using System.Collections.Generic;

namespace Mushka.Application.Suppliers.Queries.Describe;

public sealed class DescribeSupplierReadModel
{
    public DescribeSupplierDetailsReadModel Supplier { get; set; }

    public IEnumerable<DescribeSupplierContactPersonReadModel> ContactPersons { get; set; }

    public IEnumerable<DescribeSupplierPaymentCardReadModel> PaymentCards { get; set; }
}

public sealed class DescribeSupplierDetailsReadModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public string Email { get; set; }

    public string WebSite { get; set; }

    public string Notes { get; set; }

    public string Service { get; set; }

    public int SuppliesCount { get; set; }
}

public sealed class DescribeSupplierContactPersonReadModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Position { get; set; }

    public string City { get; set; }

    public string Email { get; set; }

    public string Phones { get; set; }
}

public sealed class DescribeSupplierPaymentCardReadModel
{
    public Guid Id { get; set; }

    public string Number { get; set; }

    public string Owner { get; set; }
}

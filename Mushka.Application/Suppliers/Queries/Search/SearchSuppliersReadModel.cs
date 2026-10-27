using System;
using System.Collections.Generic;

namespace Mushka.Application.Suppliers.Queries.Search;

public sealed class SearchSuppliersReadModel
{
    public int Total { get; set; }

    public IEnumerable<SearchSupplierReadModel> Items { get; set; }
}

public sealed class SearchSupplierReadModel
{
    public SearchSupplierDetailsReadModel Supplier { get; set; }

    public IEnumerable<SearchSupplierContactPersonReadModel> ContactPersons { get; set; }

    public IEnumerable<SearchSupplierPaymentCardReadModel> PaymentCards { get; set; }
}

public sealed class SearchSupplierDetailsReadModel
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

public sealed class SearchSupplierContactPersonReadModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Position { get; set; }

    public string City { get; set; }

    public string Email { get; set; }

    public string Phones { get; set; }
}

public sealed class SearchSupplierPaymentCardReadModel
{
    public Guid Id { get; set; }

    public string Number { get; set; }

    public string Owner { get; set; }
}

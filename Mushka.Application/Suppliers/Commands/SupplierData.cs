using System;

namespace Mushka.Application.Suppliers.Commands;

public sealed record SupplierData(
    string Name,
    string Address,
    string Email,
    string WebSite,
    string Service,
    string Notes);

public sealed record ContactPersonData(Guid? Id, string Name, string Email, string Phones);

public sealed record PaymentCardData(Guid? Id, string Number, string Owner);

using System;

namespace Mushka.Application.Suppliers.Commands;

public sealed record SupplierCommandModel(
    string Name,
    string Address,
    string Email,
    string WebSite,
    string Service,
    string Notes);

public sealed record ContactPersonCommandModel(Guid? Id, string Name, string Email, string Phones);

public sealed record PaymentCardCommandModel(Guid? Id, string Number, string Owner);

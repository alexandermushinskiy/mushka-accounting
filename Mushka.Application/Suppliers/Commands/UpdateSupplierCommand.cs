using System;
using System.Collections.Generic;
using Mediator;

namespace Mushka.Application.Suppliers.Commands;

public sealed record UpdateSupplierCommand(
    Guid Id,
    SupplierCommandModel Supplier,
    IReadOnlyList<ContactPersonCommandModel> ContactPersons,
    IReadOnlyList<PaymentCardCommandModel> PaymentCards) : ICommand;

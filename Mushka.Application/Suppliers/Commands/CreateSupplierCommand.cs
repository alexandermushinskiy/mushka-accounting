using System.Collections.Generic;
using Mediator;

namespace Mushka.Application.Suppliers.Commands;

public sealed record CreateSupplierCommand(
    SupplierCommandModel Supplier,
    IReadOnlyList<ContactPersonCommandModel> ContactPersons,
    IReadOnlyList<PaymentCardCommandModel> PaymentCards) : ICommand;

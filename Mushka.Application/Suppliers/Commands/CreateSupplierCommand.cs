using System.Collections.Generic;
using Mediator;

namespace Mushka.Application.Suppliers.Commands;

public sealed record CreateSupplierCommand(
    SupplierData Supplier,
    IReadOnlyList<ContactPersonData> ContactPersons,
    IReadOnlyList<PaymentCardData> PaymentCards) : ICommand;
